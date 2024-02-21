using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Application.Dependencies.Queries.GetDependency;
using RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependenciesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DependencyDetailVm>> Get([FromQuery] GetDependencyQuery query)
        => await Mediator.Send(query);
    
    [HttpPost("[action]")]
    public async Task<ActionResult<PaginatedList<DependencyVm>>> Search(SearchDependenciesWithPaginationQuery query) 
        => await Mediator.Send(query);
}