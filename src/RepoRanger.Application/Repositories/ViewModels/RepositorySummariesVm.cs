namespace RepoRanger.Application.Repositories.ViewModels;

public sealed class RepositorySummariesVm
{
    public IReadOnlyCollection<RepositorySummaryVm> RepositorySummaries { get; init; } = Array.Empty<RepositorySummaryVm>();
}