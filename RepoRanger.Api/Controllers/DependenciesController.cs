using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependenciesController : ApiControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<PaginatedList<DependencyVm>>> SearchDependenciesWithPagination(SearchDependenciesWithPaginationQuery query) 
        => await Mediator.Send(query);
}