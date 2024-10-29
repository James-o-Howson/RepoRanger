namespace RepoRanger.Domain.OutboxMessages;

public interface IOutboxMessageProcessor
{
    Task DispatchAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
}