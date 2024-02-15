namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;

public class ProjectsVm
{
    public IReadOnlyCollection<ProjectVm> Projects { get; init; }
}