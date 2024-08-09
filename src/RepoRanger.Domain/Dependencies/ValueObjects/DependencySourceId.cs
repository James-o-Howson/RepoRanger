namespace RepoRanger.Domain.Dependencies.ValueObjects;

public readonly record struct DependencySourceId(Guid Value)
{
    internal static DependencySourceId Empty => new(Guid.Empty);
    internal static DependencySourceId New => new(Guid.NewGuid());
};