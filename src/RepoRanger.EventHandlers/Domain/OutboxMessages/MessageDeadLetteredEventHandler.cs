using Microsoft.Extensions.Logging;
using RepoRanger.Abstractions.Events.Domain;
using RepoRanger.Domain.OutboxMessages.Events;

namespace RepoRanger.EventHandlers.Domain.OutboxMessages;

internal sealed class MessageDeadLetteredEventHandler : DomainEventHandler<MessageDeadLettered>
{
    private readonly ILogger<MessageDeadLetteredEventHandler> _logger;

    public MessageDeadLetteredEventHandler(ILogger<MessageDeadLetteredEventHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleAsync(MessageDeadLettered domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogWarning("OutboxMessage with Id = {Id} has been Dead Lettered", domainEvent.OutboxMessageId);
        
        return Task.CompletedTask;
    }
}