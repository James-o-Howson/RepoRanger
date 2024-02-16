using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Domain.Entities;
using RepoRanger.SourceParsing.Abstractions.Options;

namespace RepoRanger.SourceParsing.Services;

internal sealed class SourceParserService : ISourceParser
{
    private readonly ConcurrentBag<IFileContentParser> _fileContentParsers;
    private readonly SourceParserOptions _options;
    private readonly ILogger<SourceParserService> _logger;

    private IEnumerable<SourceOptions> EnabledSourceOptions => _options.Sources.Where(s => s.Enabled);

    public SourceParserService(IEnumerable<IFileContentParser> fileContentParsers,
        IOptions<SourceParserOptions> options,
        ILogger<SourceParserService> logger)
    {
        _fileContentParsers = new ConcurrentBag<IFileContentParser>(fileContentParsers);
        _logger = logger;
        _options = options.Value;
    }

    public async Task<IEnumerable<Source>> ParseAsync()
    {
        var parseTasks = EnabledSourceOptions.Select(ParseSourceAsync);
        return await Task.WhenAll(parseTasks);
    }

    private async Task<Source> ParseSourceAsync(SourceOptions sourceOptions)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);
        
        var repositoryPaths = FindRepositories(sourceOptions.WorkingDirectory);

        var repositories = await Task.WhenAll(repositoryPaths
            .Where(p => !sourceOptions.IsExcluded(p))
            .Select(ParseRepositoryAsync));

        var source = new Source(sourceOptions.Name);
        source.AddRepositories(repositories);
        
        _logger.LogInformation("Finished Parsing Source {SourceName}", sourceOptions.Name);

        return source;
    }

    private static List<string> FindRepositories(string directory)
    {
        var repositoryPaths = new List<string>();
        foreach (var subdirectory in Directory.GetDirectories(directory))
        {
            if (Directory.Exists(Path.Combine(subdirectory, ".git")))
            {
                repositoryPaths.Add(subdirectory);
            }
            else
            {
                FindRepositories(subdirectory);
            }
        }

        return repositoryPaths;
    }
    
    private async Task<Repository> ParseRepositoryAsync(string directory)
    {
        var repository = GetRepository(directory);
        
        _logger.LogInformation("Parsing Repository {RepositoryName}", repository.Name);

        var filePaths = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
        
        var parseFileTasks = filePaths.Select(filePath => ParseFileAsync(filePath, repository.DefaultBranch));
        await Task.WhenAll(parseFileTasks);
        
        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", repository.Name);

        return repository;
    }
    
    private async Task ParseFileAsync(string filePath, Branch branch)
    {
        _logger.LogDebug("Checking file {filePath}", filePath);

        var fileContentParser = _fileContentParsers.SingleOrDefault(p => p.CanParse(filePath));
        if (fileContentParser != null)
        {
            _logger.LogDebug("*** Can Parse file {FilePath} ***", filePath);

            // Don't read content or make file info unless the file path matches a parser, it's expensive.
            var content = await File.ReadAllTextAsync(filePath);
            var fileInfo = new FileInfo(filePath);
            
            await fileContentParser.ParseAsync(content, fileInfo, branch);
        }
    }
    
    private static Repository GetRepository(string repositoryPath)
    {
        using var repo = new LibGit2Sharp.Repository(repositoryPath);

        var branchName = repo.Head.UpstreamBranchCanonicalName ?? repo.Head.CanonicalName;
        var branch = new Branch(branchName, true);

        var repositoryName = new DirectoryInfo(repo.Info.WorkingDirectory).Name;
        var repository = new Repository(repositoryName, repo.Network.Remotes["origin"].Url);
        repository.AddBranch(branch);
        
        return repository;
    }
}