namespace RepoRanger.Domain.VersionControlSystems.Git;

public sealed record GitRepositoryDetail
{
    public string Name { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public string RemoteUrl { get; init; } = string.Empty;
}