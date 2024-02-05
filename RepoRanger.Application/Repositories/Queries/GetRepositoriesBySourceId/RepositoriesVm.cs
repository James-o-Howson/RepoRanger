namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

public class RepositoriesVm
{
    public IReadOnlyCollection<RepositoryVm> Repositories { get; init; }
}