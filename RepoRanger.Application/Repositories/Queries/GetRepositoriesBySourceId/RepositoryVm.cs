namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

public class RepositoryVm
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? RemoteUrl { get; init; }
}