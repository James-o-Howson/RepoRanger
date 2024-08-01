namespace RepoRanger.Application.Contracts.VersionControlSystems;

public sealed class VersionControlSystemVm
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}