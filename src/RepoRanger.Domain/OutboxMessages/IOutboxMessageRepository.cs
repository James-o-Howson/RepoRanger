namespace RepoRanger.Domain.OutboxMessages;

public interface IOutboxMessageRepository
{
    Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(int batchSize, CancellationToken cancellationToken = default);
}