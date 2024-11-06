using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;
using SharedKernel.Base;

namespace RepoRanger.Domain.OutboxMessages.Entities;

public class DeadLetterEntry : BaseEntity
{
    private DeadLetterEntry() { }

    public static DeadLetterEntry Create(OutboxMessageId failedMessageId, ProcessingFailure finalProcessingFailure, DateTimeOffset occuredAt) => new()
    {
        OccuredAt = occuredAt,
        FinalProcessingFailureId = finalProcessingFailure.Id,
        FailedMessageId = failedMessageId
    };

    public DeadLetterEntryId Id { get; } = DeadLetterEntryId.New;
    public required ProcessingFailureId FinalProcessingFailureId { get; init; }
    public ProcessingFailure FinalProcessingFailure { get; init; } = null!;
    public required OutboxMessageId FailedMessageId { get; init; }
    public OutboxMessage FailedMessage { get; init; } = null!;
    public required DateTimeOffset OccuredAt { get; init; }
}