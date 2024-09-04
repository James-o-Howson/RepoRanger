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