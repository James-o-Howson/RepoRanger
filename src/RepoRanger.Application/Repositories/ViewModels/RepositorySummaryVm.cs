namespace RepoRanger.Application.Repositories.ViewModels;

public sealed class RepositorySummaryVm
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? RemoteUrl { get; init; }
    public string? DefaultBranchName { get; init; }
    public DateTime ParseTime { get; init; }
}