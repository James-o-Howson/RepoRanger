using MediatR;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.OutboxMessages.Events;

namespace RepoRanger.EventHandlers.Domain.OutboxMessages;

internal sealed class MessageDeadLetteredEventHandler : INotificationHandler<MessageDeadLettered>
{
    private readonly ILogger<MessageDeadLetteredEventHandler> _logger;

    public MessageDeadLetteredEventHandler(ILogger<MessageDeadLetteredEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MessageDeadLettered notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("OutboxMessage with Id = {Id} has been Dead Lettered", notification.OutboxMessageId);
        
        return Task.CompletedTask;
    }
}