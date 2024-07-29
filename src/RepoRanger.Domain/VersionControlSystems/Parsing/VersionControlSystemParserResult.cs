using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public sealed class VersionControlSystemParserResult
{
    public static VersionControlSystemParserResult CreateInstance(VersionControlSystemDescriptor descriptor,
        ParsingContext context) => new()
    {
        VersionControlSystemDescriptor = descriptor,
        Context = context
    };

    public VersionControlSystemDescriptor VersionControlSystemDescriptor { get; private init; } = null!;
    public ParsingContext Context { get; private init; } = null!;
}