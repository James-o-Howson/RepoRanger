namespace RepoRanger.Application.Repositories.ViewModels;

public sealed class RepositoryVm
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? RemoteUrl { get; init; }
    public Guid DefaultBranchId { get; init; }
    public string? DefaultBranchName { get; init; }
    public DateTime ParseTime { get; init; }
}