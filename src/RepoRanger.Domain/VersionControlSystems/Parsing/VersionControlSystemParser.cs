using System.Collections.Concurrent;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public interface IVersionControlSystemParser
{
    Task<VersionControlSystemParserResult> ParseAsync(VersionControlSystemContext versionControlSystemContext, CancellationToken cancellationToken);
}

internal sealed class VersionControlSystemParser : IVersionControlSystemParser
{
    private readonly ParsingContext _parseContext;
    private readonly IRepositoryParser _repositoryParser;

    public VersionControlSystemParser(
        IEnumerable<IProjectFileParser> fileContentParsers, 
        IRepositoryParser repositoryParser)
    {
        ConcurrentQueue<IProjectFileParser> sourceFileParsers = new(fileContentParsers);
        _repositoryParser = repositoryParser;

        _parseContext = ParsingContext.Create(sourceFileParsers);
    }
    
    public async Task<VersionControlSystemParserResult> ParseAsync(VersionControlSystemContext versionControlSystemContext, CancellationToken cancellationToken) 
        => await ParseSourceAsync(versionControlSystemContext, cancellationToken);

    private async Task<VersionControlSystemParserResult> ParseSourceAsync(VersionControlSystemContext versionControlSystemContext, CancellationToken cancellationToken)
    {
        var repositories = await ParseRepositoriesAsync(versionControlSystemContext);
        var source = new VersionControlSystemDescriptor(versionControlSystemContext.Name, versionControlSystemContext.Location, repositories);
        
        return VersionControlSystemParserResult.CreateInstance(source, _parseContext);
    }
    
    private async Task<IReadOnlyCollection<RepositoryDescriptor>> ParseRepositoriesAsync(VersionControlSystemContext versionControlSystemContext)
    {
        var gitRepositories = versionControlSystemContext.LocationInfo.GetGitRepositoryPaths();

        var repositoryPaths = gitRepositories
            .Where(p => !versionControlSystemContext.IsExcluded(p));

        List<RepositoryDescriptor> descriptors = [];
        foreach (var repositoryPath in repositoryPaths)
        {
            descriptors.Add(await ParseRepositoryAsync(repositoryPath));
        }

        return descriptors;
    }

    private async Task<RepositoryDescriptor> ParseRepositoryAsync(string repositoryPath)
    {
        var gitRepository = new DirectoryInfo(repositoryPath);

        return await _repositoryParser.ParseAsync(gitRepository, _parseContext);
    }
}