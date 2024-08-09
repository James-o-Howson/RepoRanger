namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct ProjectMetadataId(Guid Value)
{
    internal static ProjectMetadataId Empty => new(Guid.Empty);
    internal static ProjectMetadataId New => new(Guid.NewGuid());
};