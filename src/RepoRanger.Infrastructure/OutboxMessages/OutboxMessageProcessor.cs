using MediatR;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.OutboxMessages;

namespace RepoRanger.Infrastructure.OutboxMessages;

internal sealed class OutboxMessageProcessor : IOutboxMessageProcessor
{
    private const int RetryThreshold = 10;
    
    private readonly ILogger<OutboxMessageProcessor> _logger;
    private readonly IMediator _mediator;
    private readonly TimeProvider _timeProvider;

    public OutboxMessageProcessor(ILogger<OutboxMessageProcessor> logger,
        IMediator mediator,
        TimeProvider timeProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _timeProvider = timeProvider;
    }

    public async Task DispatchAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            outboxMessage.StartProcessing(_timeProvider.GetUtcNow());
            _logger.LogInformation("Processing Outbox Message: {OutboxMessageId}", outboxMessage.Id);
            
            await _mediator.Publish(outboxMessage.Event, cancellationToken);
                
            outboxMessage.Succeed(_timeProvider.GetUtcNow());
            _logger.LogInformation("Finished Processing Outbox Message: {OutboxMessageId}", outboxMessage.Id);
        }
        catch (Exception exception)
        {
            outboxMessage.Fail(RetryThreshold, exception);
            _logger.LogError(exception, "Failed to Process Persisted Event: {PersistedJobName}", outboxMessage.Id);
        }
    }
}