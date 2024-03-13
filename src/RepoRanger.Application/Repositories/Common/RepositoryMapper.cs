using RepoRanger.Application.Projects.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Repositories.Common;

public static class RepositoryMapper
{
    public static IEnumerable<Repository> ToEntities(this IEnumerable<RepositoryAggregateVm> viewModels)
    {
        ArgumentNullException.ThrowIfNull(viewModels);
        return viewModels.Select(ToEntity);
    }
    
    private static Repository ToEntity(this RepositoryAggregateVm viewModel)
    {
        return Repository.Create(
            viewModel.Name, 
            viewModel.RemoteUrl, 
            viewModel.Branch, 
            viewModel.Projects.ToEntities().ToList());
    }
}