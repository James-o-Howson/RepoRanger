using Microsoft.AspNetCore.Mvc;
using RepoRanger.Abstractions.Pagination;
using RepoRanger.Contracts.ProjectDependencies;
using RepoRanger.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependencyInstancesController : ApiControllerBase
{
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(PaginatedList<ProjectDependencyVm>), 200)]
    public async Task<ActionResult<PaginatedList<ProjectDependencyVm>>> Search(SearchProjectDependenciesWithPaginationQuery query) => 
        await Mediator.Send(query);
}