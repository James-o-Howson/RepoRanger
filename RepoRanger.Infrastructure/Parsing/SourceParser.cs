using Microsoft.Extensions.Options;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Infrastructure.Parsing;

internal sealed class SourceParser : ISourceParser
{
    private readonly IEnumerable<IFileContentParser> _fileContentParsers;
    private readonly SourceParserOptions _options;
    
    private IEnumerable<SourceOptions> EnabledSourceOptions => _options.Sources.Where(s => s.Enabled);

    public SourceParser(IEnumerable<IFileContentParser> fileContentParsers, IOptions<SourceParserOptions> options)
    {
        _fileContentParsers = fileContentParsers;
        _options = options.Value;
    }

    public IEnumerable<SourceContext> Parse() => EnabledSourceOptions.Select(ParseSource);

    private SourceContext ParseSource(SourceOptions source)
    {
        var repositoryPaths = FindRepositories(source.SourceRepositoryParentDirectory);

        var sourceContext = new SourceContext
        {
            SourceName = source.Name,
            RepositoryContexts = repositoryPaths.Select(ParseRepository)
        };

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
    
    private RepositoryContext ParseRepository(string directory)
    {
        var repositoryContext = GetRepositoryContext(directory);
        var branchContext = GetBranchContext(directory);

        var filePaths = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        foreach (var filePath in filePaths)
        {
            var content = File.ReadAllText(filePath);
            var fileInfo = new FileInfo(filePath);

            var fileContentParser = _fileContentParsers.SingleOrDefault(p => p.CanParse(fileInfo));
            fileContentParser?.Parse(content, fileInfo, branchContext);
        }
        
        repositoryContext.BranchContexts.Add(branchContext);

        return repositoryContext;
    }
    
    private static RepositoryContext GetRepositoryContext(string repositoryPath)
    {
        using var repo = new LibGit2Sharp.Repository(repositoryPath);
        return new RepositoryContext
        {
            Name = repo.Info.WorkingDirectory,
            RemoteUrl = repo.Network.Remotes["origin"].Url
        };
    }

    private static BranchContext GetBranchContext(string repositoryPath)
    {
        using var repo = new LibGit2Sharp.Repository(repositoryPath);
        return new BranchContext
        {
            Name = repo.Head.UpstreamBranchCanonicalName,
            IsDefault = true
        };
    }
}