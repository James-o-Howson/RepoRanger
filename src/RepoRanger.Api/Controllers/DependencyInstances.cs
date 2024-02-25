using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;
using RepoRanger.Application.DependencyInstances.Queries.SearchDependencyInstancesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependencyInstances : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DependencyInstanceDetailVm>> Get([FromQuery] GetDependencyInstanceQuery query)
        => await Mediator.Send(query);
    
    [HttpPost("[action]")]
    public async Task<ActionResult<PaginatedList<DependencyInstanceVm>>> Search(SearchDependencyInstancesWithPaginationQuery query) 
        => await Mediator.Send(query);
}