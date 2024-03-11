using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Sources.Queries.ListSources;

namespace RepoRanger.Api.Controllers;

public sealed class SourcesController : ApiControllerBase
{
    [HttpGet] 
    [ProducesResponseType(typeof(SourcesVm), 200)]
    public async Task<ActionResult<SourcesVm>> List([FromQuery] ListSourcesQuery query) => 
        await Mediator.Send(query);
}