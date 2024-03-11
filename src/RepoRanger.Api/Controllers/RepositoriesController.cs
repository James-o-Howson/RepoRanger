using Microsoft.AspNetCore.Mvc;
using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Repositories.Queries.ListRepositories;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Api.Controllers;

public sealed class RepositoriesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(RepositoriesVm), 200)]
    public async Task<ActionResult<RepositoriesVm>> List([FromQuery] ListRepositoriesQuery query) => 
        await Mediator.Send(query);
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(RepositoriesVm), 200)]
    public async Task<ActionResult<RepositoriesVm>> GetBySourceId([FromQuery] GetRepositoriesBySourceIdQuery query) => 
        await Mediator.Send(query);
}