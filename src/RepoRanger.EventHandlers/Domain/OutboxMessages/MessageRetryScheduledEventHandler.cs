using MediatR;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.OutboxMessages.Events;

namespace RepoRanger.EventHandlers.Domain.OutboxMessages;

internal sealed class MessageRetryScheduledEventHandler : INotificationHandler<MessageRetryScheduled>
{
    private readonly ILogger<MessageRetryScheduledEventHandler> _logger;

    public MessageRetryScheduledEventHandler(ILogger<MessageRetryScheduledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MessageRetryScheduled notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Outbox Message with Id = {Id} has failed. Retrying at {RetryingAt}", 
            notification.OutboxMessageId, notification.NextRetryAt);
        
        return Task.CompletedTask;
    }
}