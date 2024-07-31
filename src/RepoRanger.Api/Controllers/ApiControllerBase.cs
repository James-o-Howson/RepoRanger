using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepoRanger.Api.Infrastructure;

namespace RepoRanger.Api.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => 
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}