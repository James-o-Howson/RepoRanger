using Microsoft.AspNetCore.Mvc;
using RepoRanger.Commands.Repositories.DeleteRepository;
using RepoRanger.Contracts.Repositories;
using RepoRanger.Queries.Repositories.GetRepositoriesByVersionControlSystemId;
using RepoRanger.Queries.Repositories.GetRepositoryById;
using RepoRanger.Queries.Repositories.ListRepositories;

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
    public async Task<ActionResult<RepositorySummariesVm>> GetByVersionControlSystemId([FromQuery] GetRepositoriesByVersionControlSystemIdQuery query) => 
        await Mediator.Send(query);
    
    [HttpDelete("[action]")]
    [ProducesResponseType(200)]
    public async Task Delete([FromQuery] DeleteRepositoryCommand query) => 
        await Mediator.Send(query);
}