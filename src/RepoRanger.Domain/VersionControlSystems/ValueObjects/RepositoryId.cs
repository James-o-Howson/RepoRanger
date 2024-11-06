using RepoRanger.SharedKernel.Abstractions;

namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct RepositoryId(Guid Value) : IId
{
    internal static RepositoryId Empty => new(Guid.Empty);
    internal static RepositoryId New => new(Guid.NewGuid());
};