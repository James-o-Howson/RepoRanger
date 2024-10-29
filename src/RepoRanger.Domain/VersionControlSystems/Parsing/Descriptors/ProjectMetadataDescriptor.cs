using RepoRanger.Domain.VersionControlSystems.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record ProjectMetadataDescriptor(string Key, string Value) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new ProjectMetadataAlternateId(Key);
}