namespace RepoRanger.Application.Projects.ViewModels;

public sealed class ProjectsVm
{
    public IReadOnlyCollection<ProjectVm> Projects { get; init; } = Array.Empty<ProjectVm>();
}