using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public readonly record struct VersionControlSystemId(Guid Value) : IId
{
    internal static VersionControlSystemId Empty => new(Guid.Empty);
    internal static VersionControlSystemId New => new(Guid.NewGuid());
};