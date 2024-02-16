namespace RepoRanger.Application.Projects.ViewModels;

public sealed class ProjectVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public int DependencyCount { get; init; }
}