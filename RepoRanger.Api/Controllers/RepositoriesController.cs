using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

namespace RepoRanger.Api.Controllers;

public sealed class RepositoriesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RepositoriesVm>> Get(GetRepositoriesBySourceIdQuery query)
    {
        return await Mediator.Send(query);
    }
}