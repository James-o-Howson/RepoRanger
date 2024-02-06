using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Sources.Queries.GetSourceDetails;

namespace RepoRanger.App.Controllers;

public sealed class SourcesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SourceDetailsVm>> GetSourceDetails([FromQuery] GetSourceDetailsQuery query)
    {
        return await Mediator.Send(query);
    }
}