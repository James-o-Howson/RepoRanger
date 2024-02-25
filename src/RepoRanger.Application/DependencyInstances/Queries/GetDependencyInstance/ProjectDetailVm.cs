namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public class ProjectDetailVm
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
}