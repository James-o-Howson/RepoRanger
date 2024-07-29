using Microsoft.Extensions.Options;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class VersionControlSystemParserService : IVersionControlSystemParserService
{
    private readonly IVersionControlSystemParser _versionControlSystemParser;
    private readonly ISourceParserResultHandler _resultHandler;
    private readonly VersionControlSystemContexts _options;

    public VersionControlSystemParserService(
        IOptions<VersionControlSystemContexts> options,
        IVersionControlSystemParser versionControlSystemParser,
        ISourceParserResultHandler resultHandler)
    {
        _versionControlSystemParser = versionControlSystemParser;
        _resultHandler = resultHandler;
        _options = options.Value;
    }

    private IEnumerable<VersionControlSystemContext> EnabledSourceOptions => 
        _options.Values.Where(s => s.Enabled);

    public async Task ParseAsync(CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(
            EnabledSourceOptions.Select(context => _versionControlSystemParser.ParseAsync(context, cancellationToken)));

        await _resultHandler.HandleAsync(results, cancellationToken);
    }
}