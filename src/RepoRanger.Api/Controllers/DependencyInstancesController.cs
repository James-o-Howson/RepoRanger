using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Contracts.ProjectDependencies;
using RepoRanger.Application.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependencyInstancesController : ApiControllerBase
{
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(PaginatedList<ProjectDependencyVm>), 200)]
    public async Task<ActionResult<PaginatedList<ProjectDependencyVm>>> Search(SearchProjectDependenciesWithPaginationQuery query) => 
        await Mediator.Send(query);
}