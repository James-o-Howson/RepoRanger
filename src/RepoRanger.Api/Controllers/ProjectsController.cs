using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Contracts.Projects;
using RepoRanger.Application.Queries.Projects.GetProjectsByDependency;
using RepoRanger.Application.Queries.Projects.GetProjectsByRepositoryIds;
using RepoRanger.Application.Queries.Projects.ListProjects;

namespace RepoRanger.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProjectsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ProjectsVm), 200)]
    public async Task<ActionResult<ProjectsVm>> List([FromQuery] ListProjectsQuery query) => 
        await Mediator.Send(query);
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(ProjectsVm), 200)]
    public async Task<ActionResult<ProjectsVm>> GetByRepositoryIds([FromQuery] GetProjectsByRepositoryIdsQuery query) => 
        await Mediator.Send(query);
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(ProjectsVm), 200)]
    public async Task<ActionResult<ProjectsVm>> GetByDependency([FromQuery] GetProjectsByDependencyQuery query) => 
        await Mediator.Send(query);
}