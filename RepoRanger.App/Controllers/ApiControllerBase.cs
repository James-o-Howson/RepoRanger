using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepoRanger.App.Filters;

namespace RepoRanger.App.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}