using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record ProjectMetadataDescriptor(string Key, string Value) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new ProjectMetadataAlternateId(Key);
}