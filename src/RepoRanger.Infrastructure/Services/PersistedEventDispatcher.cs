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

    public async Task DispatchAsync(PersistedEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            @event.StartProcessing(_timeProvider.GetUtcNow());
            _logger.LogInformation("Processing Persisted Event: {PersistedEventId}", @event.Id);

            await _mediator.Publish(@event, cancellationToken);
                
            @event.Succeed(_timeProvider.GetUtcNow());
            _logger.LogInformation("Finished Processing Persisted Event: {PersistedEventId}", @event.Id);
        }
        catch (Exception exception)
        {
            @event.Retry(RetryThreshold, exception);
            _logger.LogError(exception, "Failed to Process Persisted Event: {PersistedJobName}", @event.Id);
        }
    }
}