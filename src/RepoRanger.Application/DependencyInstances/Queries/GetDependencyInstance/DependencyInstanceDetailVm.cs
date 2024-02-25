namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public class DependencyInstanceDetailVm
{
    public Guid Id { get; set; }
    public string Source { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public IEnumerable<RepositoryDetailVm> Repositories { get; set; }
}