using RepoRanger.Domain.Common;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public sealed class ProjectMetadata : Entity, IAlternateKeyProvider
{
    private ProjectMetadata() { }

    public static ProjectMetadata Create(string key, string value) => new()
    {
        Key = key,
        Value = value
    };

    public Guid Id { get; } = Guid.NewGuid();
    public string Key { get; private init; } = string.Empty;
    public string Value { get; private init; } = string.Empty;
    public AlternateKey GetAlternateKey => new ProjectMetadataAlternateKey(Key);
}