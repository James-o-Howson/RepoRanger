using RepoRanger.SharedKernel.Abstractions;

namespace RepoRanger.Domain.Dependencies.ValueObjects;

public readonly record struct DependencyId(Guid Value) : IId
{
    internal static DependencyId Empty => new(Guid.Empty);
    internal static DependencyId New => new(Guid.NewGuid());
};