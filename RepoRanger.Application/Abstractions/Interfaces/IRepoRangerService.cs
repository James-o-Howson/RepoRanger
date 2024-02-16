using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Repositories.ViewModels;
using RepoRanger.Application.Sources.Queries.ListSources;

namespace RepoRanger.Application.Abstractions.Interfaces;

public interface IRepoRangerService
{
    Task<SourcesVm?> ListSources();
    Task<RepositoriesVm?> GetRepositoriesAsync(GetRepositoriesBySourceIdQuery query);
}