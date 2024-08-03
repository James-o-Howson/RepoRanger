using Microsoft.Extensions.Options;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing;

internal sealed class VcsParserService : IVersionControlSystemParserService
{
    private readonly IVersionControlSystemParser _versionControlSystemParser;
    private readonly IVcsParserResultHandler _resultHandler;
    private readonly VersionControlSystemContexts _options;

    public VcsParserService(
        IOptions<VersionControlSystemContexts> options,
        IVersionControlSystemParser versionControlSystemParser,
        IVcsParserResultHandler resultHandler)
    {
        _versionControlSystemParser = versionControlSystemParser;
        _resultHandler = resultHandler;
        _options = options.Value;
    }

    private IEnumerable<VersionControlSystemContext> EnabledVcsOptions => 
        _options.Values.Where(s => s.Enabled);

    public async Task ParseAsync(CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(
            EnabledVcsOptions.Select(context => _versionControlSystemParser.ParseAsync(context, cancellationToken)));

        await _resultHandler.HandleAsync(results, cancellationToken);
    }
}