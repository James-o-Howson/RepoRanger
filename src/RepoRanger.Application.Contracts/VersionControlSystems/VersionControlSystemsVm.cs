namespace RepoRanger.Application.Contracts.VersionControlSystems;

public sealed class VersionControlSystemsVm
{
    public IReadOnlyCollection<VersionControlSystemVm> VersionControlSystems { get; init; } = Array.Empty<VersionControlSystemVm>();
}