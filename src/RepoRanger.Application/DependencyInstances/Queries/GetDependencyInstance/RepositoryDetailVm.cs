namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public class RepositoryDetailVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<ProjectDetailVm> Projects { get; init; }
}