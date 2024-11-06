namespace RepoRanger.Domain.OutboxMessages;

public interface IOutboxMessageDispatcher
{
    Task ProcessAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
    Task ProcessAsync(CancellationToken cancellationToken = default);
}