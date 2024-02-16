namespace RepoRanger.Application.Repositories.ViewModels;

public sealed class RepositoriesVm
{
    public IReadOnlyCollection<RepositoryVm> Repositories { get; init; }
}