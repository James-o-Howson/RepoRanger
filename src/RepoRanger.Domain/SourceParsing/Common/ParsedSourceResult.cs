using RepoRanger.Domain.Entities;

namespace RepoRanger.Domain.SourceParsing.Common;

public sealed class ParsedSourceResult
{
    public static ParsedSourceResult CreateInstance(Source? existing, Source parsed) => new()
    {
        Existing = existing,
        Parsed = parsed
    };

    public Source? Existing { get; private init; }
    public Source Parsed { get; private init; } = null!;

    public bool IsNewSource => Existing is null;
}