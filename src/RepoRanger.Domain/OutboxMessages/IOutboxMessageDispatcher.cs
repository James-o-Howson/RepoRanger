namespace RepoRanger.Domain.OutboxMessages;

public interface IOutboxMessageDispatcher
{
    Task DispatchAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
}