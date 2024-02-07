using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Sources.Queries.ListSources;

namespace RepoRanger.Application.Abstractions.Interfaces;

public interface IRepoRangerService
{
    Task<SourcesViewModel?> ListSources();
    Task<RepositoriesVm?> GetRepositoriesAsync(GetRepositoriesBySourceIdQuery query);
}