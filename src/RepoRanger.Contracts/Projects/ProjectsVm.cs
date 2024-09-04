namespace RepoRanger.Contracts.Projects;

public sealed class ProjectsVm
{
    public IReadOnlyCollection<ProjectVm> Projects { get; init; } = Array.Empty<ProjectVm>();
}