using Microsoft.Extensions.Options;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Domain.SourceParsing.Common;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class SourceParserService : ISourceParserService
{
    private readonly ISourceParser _sourceParser;
    private readonly ISourceParserResultHandler _resultHandler;
    private readonly SourceContexts _options;

    public SourceParserService(
        IOptions<SourceContexts> options,
        ISourceParser sourceParser,
        ISourceParserResultHandler resultHandler)
    {
        _sourceParser = sourceParser;
        _resultHandler = resultHandler;
        _options = options.Value;
    }

    private IEnumerable<SourceContext> EnabledSourceOptions => 
        _options.Sources.Where(s => s.Enabled);

    public async Task ParseAsync(CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(
            EnabledSourceOptions.Select(options => _sourceParser.ParseAsync(options, cancellationToken)));

        await _resultHandler.HandleAsync(results, cancellationToken);
    }
}