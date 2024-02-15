namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;

public class ProjectVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
}