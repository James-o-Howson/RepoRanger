using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Application.Abstractions.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest>> _logger;
    private readonly IUser _user;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger, IUser user)
    {
        _logger = logger;
        _user = user;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.UserId;
        var userName = string.Empty;
        
        _logger.LogInformation("RepoRanger Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
        
        return Task.CompletedTask;
    }
}
