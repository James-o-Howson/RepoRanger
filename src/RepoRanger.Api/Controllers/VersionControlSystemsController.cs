using Microsoft.AspNetCore.Mvc;
using RepoRanger.Contracts.VersionControlSystems;
using RepoRanger.Queries.VersionControlSystems.ListVersionControlSystems;

namespace RepoRanger.Api.Controllers;

public sealed class VersionControlSystemsController : ApiControllerBase
{
    [HttpGet] 
    [ProducesResponseType(typeof(VersionControlSystemsVm), 200)]
    public async Task<ActionResult<VersionControlSystemsVm>> List([FromQuery] ListVersionControlSystemsQuery query) => 
        await Mediator.Send(query);
}