using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;
using RepoRanger.Application.Projects.Queries.ListProjects;
using RepoRanger.Application.Projects.ViewModels;

namespace RepoRanger.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProjectsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProjectsVm>> List([FromQuery] ListProjectsQuery query) 
        => await Mediator.Send(query);
    
    [HttpGet("[action]")]
    public async Task<ActionResult<ProjectsVm>> GetByRepositoryId([FromQuery] GetProjectsByRepositoryIdQuery query) 
        => await Mediator.Send(query);
}