using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

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

    public async Task<IEnumerable<SourceContext>> ParseAsync()
    {
        var parseTasks = ParallelEnabledSourceOptions.Select(ParseSourceAsync);
        
        return await Task.WhenAll(parseTasks);
    }

    private IEnumerable<SourceOptions> ParallelEnabledSourceOptions =>
        EnabledSourceOptions
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);

    private async Task<SourceContext> ParseSourceAsync(SourceOptions source)
    {
        _logger.LogInformation("Parsing Source {SourceName}", source.Name);
        
        var repositoryPaths = FindRepositories(source.SourceRepositoryParentDirectory);

        var sourceContext = new SourceContext
        {
            Name = source.Name,
            RepositoryContexts = await Task.WhenAll(repositoryPaths.Select(ParseRepositoryAsync))
        };
        
        _logger.LogInformation("Finished Parsing Source {SourceName}", source.Name);

        return sourceContext;
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
    
    private async Task<RepositoryContext> ParseRepositoryAsync(string directory)
    {
        var repositoryContext = GetRepositoryContext(directory);
        
        _logger.LogInformation("Parsing Repository {RepositoryName}", repositoryContext.Name);

        var filePaths = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        var parseFileTasks = filePaths.Select(filePath => ParseFileAsync(filePath, repositoryContext.DefaultBranch));
        await Task.WhenAll(parseFileTasks);
        
        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", repositoryContext.Name);

        return repositoryContext;
    }
    
    private async Task ParseFileAsync(string filePath, BranchContext branchContext)
    {
        var content = await File.ReadAllTextAsync(filePath);
        var fileInfo = new FileInfo(filePath);
        
        _logger.LogDebug("Checking file {FilePath}", fileInfo.FullName);

        var fileContentParser = _fileContentParsers.SingleOrDefault(p => p.CanParse(fileInfo));
        if (fileContentParser != null)
        {
            _logger.LogDebug("*** Can Parse file {FilePath} ***", fileInfo.FullName);
            await fileContentParser.ParseAsync(content, fileInfo, branchContext);
        }
    }
    
    private static RepositoryContext GetRepositoryContext(string repositoryPath)
    {
        using var repo = new LibGit2Sharp.Repository(repositoryPath);
        
        var branchContext = new BranchContext
        {
            Name = repo.Head.UpstreamBranchCanonicalName,
            IsDefault = true
        };
        
        var repositoryContext = new RepositoryContext
        {
            Name = repo.Info.WorkingDirectory,
            RemoteUrl = repo.Network.Remotes["origin"].Url,
            BranchContexts = [branchContext]
        };

        return repositoryContext;
    }
}