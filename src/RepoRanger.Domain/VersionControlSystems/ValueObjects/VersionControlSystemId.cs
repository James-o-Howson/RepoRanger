namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct VersionControlSystemId(Guid Value)
{
    internal static VersionControlSystemId Empty => new(Guid.Empty);
    internal static VersionControlSystemId New => new(Guid.NewGuid());
};