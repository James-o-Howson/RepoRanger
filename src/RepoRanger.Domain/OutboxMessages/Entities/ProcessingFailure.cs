using RepoRanger.Domain.OutboxMessages.ValueObjects;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;
using SharedKernel.Base;

namespace RepoRanger.Domain.OutboxMessages.Entities;

public class ProcessingFailure : BaseEntity
{
    private ProcessingFailure() { }

    public static ProcessingFailure Create(Error error, DateTimeOffset occuredAt, OutboxMessageId outboxMessageId) => new()
    {
        Error = error,
        OccuredAt = occuredAt,
        OutboxMessageId = outboxMessageId,
    };

    public ProcessingFailureId Id { get; } = ProcessingFailureId.New;
    public required Error Error { get; init; }
    public required DateTimeOffset OccuredAt { get; init; }
    public required OutboxMessageId OutboxMessageId { get; init; }
    public OutboxMessage OutboxMessage { get; init; } = null!;
}