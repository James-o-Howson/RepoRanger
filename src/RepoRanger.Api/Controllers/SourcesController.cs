using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Sources.Queries.ListSources;

namespace RepoRanger.Api.Controllers;

public sealed class SourcesController : ApiControllerBase
{
    [HttpGet] 
    public async Task<ActionResult<SourcesVm>> List([FromQuery] ListSourcesQuery query)
    {
        return await Mediator.Send(query);
    }
}