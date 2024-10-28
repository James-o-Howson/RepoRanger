using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.Dependencies.ValueObjects;

public readonly record struct DependencySourceId(Guid Value) : IId
{
    internal static DependencySourceId Empty => new(Guid.Empty);
    internal static DependencySourceId New => new(Guid.NewGuid());
};