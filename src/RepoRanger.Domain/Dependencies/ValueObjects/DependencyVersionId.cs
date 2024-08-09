namespace RepoRanger.Domain.Dependencies.ValueObjects;

public readonly record struct DependencyVersionId(Guid Value)
{
    internal static DependencyVersionId Empty => new(Guid.Empty);
    internal static DependencyVersionId New => new(Guid.NewGuid());
};