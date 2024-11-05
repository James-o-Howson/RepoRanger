using RepoRanger.Domain.Events;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

namespace RepoRanger.Domain.OutboxMessages.Events;

public sealed class MessageRetryScheduled : DomainEvent
{
    public OutboxMessageId OutboxMessageId { get; }
    public DateTimeOffset? NextRetryAt { get; }

    public MessageRetryScheduled(OutboxMessageId outboxMessageId, 
        DateTimeOffset? nextRetryAt) : base(DateTimeOffset.UtcNow)
    {
        OutboxMessageId = outboxMessageId;
        NextRetryAt = nextRetryAt;
    }
}