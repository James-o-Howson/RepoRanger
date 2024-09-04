namespace RepoRanger.Contracts.Repositories;

public sealed class RepositorySummariesVm
{
    public IReadOnlyCollection<RepositorySummaryVm> RepositorySummaries { get; init; } = Array.Empty<RepositorySummaryVm>();
}