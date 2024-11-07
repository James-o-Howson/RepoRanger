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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepositorySummaryVm>> GetById(int id)
    {
        var result = await Mediator.Send(new GetRepositoryByIdQuery(id));
        
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RepositorySummariesVm>> List([FromQuery] ListRepositoriesQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepositorySummariesVm>> GetByVersionControlSystemId([FromQuery] GetRepositoriesByVersionControlSystemIdQuery query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }

    [HttpDelete("[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromQuery] DeleteRepositoryCommand query)
    {
        await Mediator.Send(query);
        
        return NoContent();
    }
}