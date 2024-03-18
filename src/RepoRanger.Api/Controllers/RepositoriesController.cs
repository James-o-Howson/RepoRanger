using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Repositories.Queries.GetRepositoryById;
using RepoRanger.Application.Repositories.Queries.ListRepositories;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Api.Controllers;

public sealed class RepositoriesController : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RepositorySummaryVm), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RepositorySummaryVm>> GetById(int id) => 
        await Mediator.Send(new GetRepositoryByIdQuery(id));
    
    [HttpGet]
    [ProducesResponseType(typeof(RepositorySummariesVm), 200)]
    public async Task<ActionResult<RepositorySummariesVm>> List([FromQuery] ListRepositoriesQuery query) => 
        await Mediator.Send(query);
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(RepositorySummariesVm), 200)]
    public async Task<ActionResult<RepositorySummariesVm>> GetBySourceId([FromQuery] GetRepositoriesBySourceIdQuery query) => 
        await Mediator.Send(query);
}