using Microsoft.AspNetCore.Mvc;
using RepoRanger.Contracts.Projects;
using RepoRanger.Queries.Projects.GetProjectsByDependency;
using RepoRanger.Queries.Projects.GetProjectsByRepositoryIds;
using RepoRanger.Queries.Projects.ListProjects;

namespace RepoRanger.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProjectsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectsVm>> List([FromQuery] ListProjectsQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }

    [HttpGet("[action]")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectsVm>> GetByRepositoryIds([FromQuery] GetProjectsByRepositoryIdsQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }

    [HttpGet("[action]")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectsVm>> GetByDependency([FromQuery] GetProjectsByDependencyQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
}