﻿using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

namespace RepoRanger.Api.Controllers;

public sealed class DependenciesController : ApiControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<PaginatedList<DependencyVm>>> Search(SearchDependenciesWithPaginationQuery query) 
        => await Mediator.Send(query);
}