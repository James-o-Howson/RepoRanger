namespace RepoRanger.Domain.OutboxMessages;

public interface IOutboxMessageProcessor
{
    Task ProcessAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
    Task ProcessAsync(CancellationToken cancellationToken = default);
}