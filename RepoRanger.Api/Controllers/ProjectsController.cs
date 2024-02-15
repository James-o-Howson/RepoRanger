using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;

namespace RepoRanger.Api.Controllers;

public sealed class ProjectsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProjectsVm>> Get([FromQuery] GetProjectsByRepositoryIdQuery query)
    {
        return await Mediator.Send(query);
    }
}