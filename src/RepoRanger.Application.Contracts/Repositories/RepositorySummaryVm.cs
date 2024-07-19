namespace RepoRanger.Application.Contracts.Repositories;

public sealed class RepositorySummaryVm
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? RemoteUrl { get; init; }
    public string? DefaultBranchName { get; init; }
    public DateTime ParseTime { get; init; }
}