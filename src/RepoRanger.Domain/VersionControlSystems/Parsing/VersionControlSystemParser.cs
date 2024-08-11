using System.Collections.Concurrent;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public interface IVersionControlSystemParser
{
    Task<IReadOnlyCollection<VersionControlSystemDescriptor>> ParseAsync(IEnumerable<VersionControlSystemContext> contexts,
        CancellationToken cancellationToken);
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

    public async Task<IReadOnlyCollection<VersionControlSystemDescriptor>> ParseAsync(IEnumerable<VersionControlSystemContext> contexts,
        CancellationToken cancellationToken) =>
        await Task.WhenAll(
            contexts.Select(context => ParseAsync(context, cancellationToken)));

    public async Task<VersionControlSystemDescriptor> ParseAsync(VersionControlSystemContext context, CancellationToken cancellationToken)
    {
        var repositoryDescriptors = await ParseRepositoriesAsync(context);
        return CreateDescriptor(context, repositoryDescriptors);
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

    private static VersionControlSystemDescriptor CreateDescriptor(VersionControlSystemContext context,
        IReadOnlyCollection<RepositoryDescriptor> repositoryDescriptors) => 
        new(context.Name, context.Location, repositoryDescriptors);
}