using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Contracts.Sources;
using RepoRanger.Application.Queries.Sources.ListSources;

namespace RepoRanger.Api.Controllers;

public sealed class SourcesController : ApiControllerBase
{
    [HttpGet] 
    [ProducesResponseType(typeof(SourcesVm), 200)]
    public async Task<ActionResult<SourcesVm>> List([FromQuery] ListSourcesQuery query) => 
        await Mediator.Send(query);
}