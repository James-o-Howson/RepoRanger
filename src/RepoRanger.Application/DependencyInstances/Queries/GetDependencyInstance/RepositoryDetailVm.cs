namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public class RepositoryDetailVm
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IEnumerable<ProjectDetailVm> Projects { get; init; } = Array.Empty<ProjectDetailVm>();
}