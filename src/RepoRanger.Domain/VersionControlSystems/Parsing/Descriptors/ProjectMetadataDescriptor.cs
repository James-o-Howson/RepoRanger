using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record ProjectMetadataDescriptor(string Key, string Value) : IAlternateKeyProvider
{
    public AlternateKey GetAlternateKey => new ProjectMetadataAlternateKey(Key);
}