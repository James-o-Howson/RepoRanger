using RepoRanger.Domain.Entities;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class ParsedSourceResult
{
    private ParsedSourceResult() { }

    public static ParsedSourceResult CreateInstance(Source? existing, Source parsed) => new()
    {
        Existing = existing,
        Parsed = parsed
    };

    public Source? Existing { get; init; }
    public Source Parsed { get; init; } = null!;

    public bool IsNewSource => Existing is null;
}