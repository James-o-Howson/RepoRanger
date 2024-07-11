using System.Collections.Concurrent;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing.Common;

namespace RepoRanger.Domain.SourceParsing;

public interface ISourceParser
{
    Task<ParsedSourceResult> ParseAsync(SourceContext sourceContext, CancellationToken cancellationToken);
}

internal sealed class SourceParser : ISourceParser
{
    private readonly ParsingContext _parseContext;
    private readonly ISourceRepository _sourceRepository;
    private readonly IRepositoryParser _repositoryParser;

    public SourceParser(
        IEnumerable<ISourceFileParser> fileContentParsers, 
        IRepositoryParser repositoryParser, 
        ISourceRepository sourceRepository)
    {
        ConcurrentQueue<ISourceFileParser> sourceFileParsers = new(fileContentParsers);
        _repositoryParser = repositoryParser;
        _sourceRepository = sourceRepository;

        _parseContext = ParsingContext.Create(sourceFileParsers);
    }
    
    public async Task<ParsedSourceResult> ParseAsync(SourceContext sourceContext, CancellationToken cancellationToken) 
        => await ParseSourceAsync(sourceContext, cancellationToken);

    private async Task<ParsedSourceResult> ParseSourceAsync(SourceContext sourceContext, CancellationToken cancellationToken)
    {
        var repositories = await ParseRepositoriesAsync(sourceContext);
        var source = Source.Create(sourceContext.Name, sourceContext.Location, repositories);
        
        var existing = await _sourceRepository.GetSourceAsync(sourceContext.Name, cancellationToken);

        return ParsedSourceResult.CreateInstance(existing, source);
    }
    
    private async Task<IEnumerable<Repository>> ParseRepositoriesAsync(SourceContext sourceContext)
    {
        var gitRepositories = sourceContext.LocationInfo.GetGitRepositories();

        return await Task.WhenAll(gitRepositories
            .Where(p => !sourceContext.IsExcluded(p))
            .Select(ParseRepositoryAsync));
    }

    private async Task<Repository> ParseRepositoryAsync(string repositoryPath)
    {
        var gitRepository = new DirectoryInfo(repositoryPath);

        return await _repositoryParser.ParseAsync(gitRepository, _parseContext);
    }
}