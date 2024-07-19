namespace RepoRanger.Application.Contracts.DependencyInstances;

public class DependencyInstanceVm
{
    public Guid Id { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string RepositoryName { get; set; } = string.Empty;
}