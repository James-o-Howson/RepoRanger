namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct ProjectDependencyId(Guid Value)
{
    internal static ProjectDependencyId Empty => new(Guid.Empty);
    internal static ProjectDependencyId New => new(Guid.NewGuid());
};