using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;
using RepoRanger.Application.DependencyInstances.Queries.SearchDependencyInstancesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependencyInstancesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(DependencyInstanceDetailVm), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<DependencyInstanceDetailVm>> Get([FromQuery] GetDependencyInstanceQuery query) => 
        await Mediator.Send(query);
    
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(PaginatedList<DependencyInstanceVm>), 200)]
    public async Task<ActionResult<PaginatedList<DependencyInstanceVm>>> Search(SearchDependencyInstancesWithPaginationQuery query) => 
        await Mediator.Send(query);
}