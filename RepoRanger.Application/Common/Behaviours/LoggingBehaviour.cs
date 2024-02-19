using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Common.Interfaces;

namespace RepoRanger.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;
        var userName = string.Empty;
        
        _logger.LogInformation("RepoRanger Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
        
        return Task.CompletedTask;
    }
}
