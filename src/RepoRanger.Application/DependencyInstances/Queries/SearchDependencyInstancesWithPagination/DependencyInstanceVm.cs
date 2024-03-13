namespace RepoRanger.Application.DependencyInstances.Queries.SearchDependencyInstancesWithPagination;

public class DependencyInstanceVm
{
    public int Id { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string RepositoryName { get; set; } = string.Empty;
}