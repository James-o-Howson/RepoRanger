using Microsoft.AspNetCore.Mvc;
using RepoRanger.Contracts.VersionControlSystems;
using RepoRanger.Queries.VersionControlSystems.ListVersionControlSystems;

namespace RepoRanger.Api.Controllers;

public sealed class VersionControlSystemsController : ApiControllerBase
{
    [HttpGet] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VersionControlSystemsVm>> List([FromQuery] ListVersionControlSystemsQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
}