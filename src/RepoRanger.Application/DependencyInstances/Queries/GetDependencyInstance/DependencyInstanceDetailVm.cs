namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public class DependencyInstanceDetailVm
{
    public Guid Id { get; set; }
    public string Source { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public IEnumerable<RepositoryDetailVm> Repositories { get; set; } = Array.Empty<RepositoryDetailVm>();
}