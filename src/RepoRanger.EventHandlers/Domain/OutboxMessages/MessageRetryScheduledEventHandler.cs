using Microsoft.Extensions.Logging;
using RepoRanger.Abstractions.Events.Domain;
using RepoRanger.Domain.OutboxMessages.Events;

namespace RepoRanger.EventHandlers.Domain.OutboxMessages;

internal sealed class MessageRetryScheduledEventHandler : DomainEventHandler<MessageRetryScheduled>
{
    private readonly ILogger<MessageRetryScheduledEventHandler> _logger;

    public MessageRetryScheduledEventHandler(ILogger<MessageRetryScheduledEventHandler> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleAsync(MessageRetryScheduled domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Outbox Message with Id = {Id} has failed. Retrying at {RetryingAt}", 
            domainEvent.OutboxMessageId, domainEvent.NextRetryAt);
        
        return Task.CompletedTask;
    }
}