using Microsoft.AspNetCore.Mvc;
using RepoRanger.Abstractions.Pagination;
using RepoRanger.Contracts.ProjectDependencies;
using RepoRanger.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependencyInstancesController : ApiControllerBase
{
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedList<ProjectDependencyVm>>> Search(SearchProjectDependenciesWithPaginationQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
}