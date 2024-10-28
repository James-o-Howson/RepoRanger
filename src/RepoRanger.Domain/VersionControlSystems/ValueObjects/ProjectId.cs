using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct ProjectId(Guid Value) : IId
{
    internal static ProjectId Empty => new(Guid.Empty);
    internal static ProjectId New => new(Guid.NewGuid());
};