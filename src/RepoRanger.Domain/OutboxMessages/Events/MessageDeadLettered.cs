using RepoRanger.Domain.Events;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

namespace RepoRanger.Domain.OutboxMessages.Events;

public sealed class MessageDeadLettered : DomainEvent
{
    public OutboxMessageId OutboxMessageId { get; }
    public DeadLetterEntryId DeadLetterEntryId { get; }

    public MessageDeadLettered(OutboxMessageId outboxMessageId, DeadLetterEntryId deadLetterEntryId) : 
        base(DateTimeOffset.UtcNow)
    {
        OutboxMessageId = outboxMessageId;
        DeadLetterEntryId = deadLetterEntryId;
    }
}