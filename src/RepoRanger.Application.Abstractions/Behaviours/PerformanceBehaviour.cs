using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Application.Abstractions.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IUser _user;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUser user)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _user = user;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds <= 500) return response;
        
        var requestName = typeof(TRequest).Name;
        var userId = _user.UserId;

        _logger.LogWarning("RepoRanger Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
            requestName, elapsedMilliseconds, userId, request);

        return response;
    }
}
