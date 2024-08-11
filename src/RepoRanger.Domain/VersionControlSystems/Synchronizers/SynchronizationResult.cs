namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

public sealed class SynchronizationResult
{
    public IReadOnlyCollection<VersionControlSystem> ToAdd { get; init; } = [];
    public IReadOnlyCollection<VersionControlSystem> ToRemove { get; init; } = [];
    public IReadOnlyCollection<VersionControlSystem> Updated { get; init; } = [];
}