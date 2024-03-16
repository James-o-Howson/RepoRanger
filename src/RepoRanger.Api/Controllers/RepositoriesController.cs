using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Repositories.Queries.ListRepositories;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Api.Controllers;

public sealed class RepositoriesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(RepositorySummariesVm), 200)]
    public async Task<ActionResult<RepositorySummariesVm>> List([FromQuery] ListRepositoriesQuery query) => 
        await Mediator.Send(query);
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(RepositorySummariesVm), 200)]
    public async Task<ActionResult<RepositorySummariesVm>> GetBySourceId([FromQuery] GetRepositoriesBySourceIdQuery query) => 
        await Mediator.Send(query);
}