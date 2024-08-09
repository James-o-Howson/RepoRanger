using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public sealed class ProjectMetadata : Entity, IAlternateKeyProvider
{
    private ProjectMetadata() { }

    public static ProjectMetadata Create(string key, string value) => new()
    {
        Key = key,
        Value = value
    };

    public ProjectMetadataId Id { get; } = ProjectMetadataId.New;
    public string Key { get; private init; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public AlternateKey GetAlternateKey => new ProjectMetadataAlternateKey(Key);

    public void Update(string value)
    {
        DomainException.ThrowIfNullOrEmpty(value);
        Value = value;
    }
}