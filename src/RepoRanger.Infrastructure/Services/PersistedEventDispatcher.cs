using MediatR;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.PersistedEvents;

namespace RepoRanger.Infrastructure.Services;

internal sealed class PersistedEventDispatcher : IPersistedEventDispatcher
{
    private const int RetryThreshold = 10;
    
    private readonly ILogger<PersistedEventDispatcher> _logger;
    private readonly IMediator _mediator;
    private readonly TimeProvider _timeProvider;

    public PersistedEventDispatcher(ILogger<PersistedEventDispatcher> logger, IMediator mediator, TimeProvider timeProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _timeProvider = timeProvider;
    }

    public async Task DispatchAsync(PersistedEvent persistedEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            persistedEvent.StartProcessing(_timeProvider.GetUtcNow());
            _logger.LogInformation("Processing Persisted Event: {PersistedEventId}", persistedEvent.Id);

            await _mediator.Publish(persistedEvent.Event(), cancellationToken);
                
            persistedEvent.Succeed(_timeProvider.GetUtcNow());
            _logger.LogInformation("Finished Processing Persisted Event: {PersistedEventId}", persistedEvent.Id);
        }
        catch (Exception exception)
        {
            persistedEvent.Retry(RetryThreshold, exception);
            _logger.LogError(exception, "Failed to Process Persisted Event: {PersistedJobName}", persistedEvent.Id);
        }
    }
}