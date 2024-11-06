using RepoRanger.Domain.Exceptions;
using RepoRanger.Domain.OutboxMessages.Entities;
using RepoRanger.Domain.OutboxMessages.Events;
using RepoRanger.Domain.OutboxMessages.ValueObjects;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;
using RepoRanger.SharedKernel.Base;
using RepoRanger.SharedKernel.Events.Integration;

namespace RepoRanger.Domain.OutboxMessages;

public class OutboxMessage : BaseEntity
{
    private readonly List<ProcessingFailure> _failures = [];

    private OutboxMessage() { }
    
    public static OutboxMessage Create(IntegrationEvent integrationEvent, DateTimeOffset createdAt) => new()
    {
        Data = OutboxMessageData.From(integrationEvent),
        EventType = EventType.From(integrationEvent),
        Status = ProcessingStatus.Pending,
        RetryPolicy = RetryPolicy.Default,
        Metadata = ProcessingMetadata.Initial,
        CreatedAt = createdAt.DateTime,
    };
    
    public OutboxMessageId Id { get; } = OutboxMessageId.New;
    public required OutboxMessageData Data { get; init; }
    public required EventType EventType { get; init; }
    public required RetryPolicy RetryPolicy { get; init; }
    public ProcessingStatus Status { get; private set; } = ProcessingStatus.Pending;
    public ProcessingMetadata Metadata { get; private set; } = ProcessingMetadata.Initial;
    public IReadOnlyCollection<ProcessingFailure> Failures => _failures.AsReadOnly();
    public DeadLetterEntry? DeadLetterEntry { get; private set; }
    
    // Unfortunately Sqlite cannot sort by DateTimeOffset via Entity Framework
    public required DateTime CreatedAt { get; init; } 

    public IntegrationEvent Event => Data.ToIntegrationEvent(EventType);
    
    public void StartProcessing(DateTimeOffset startedAt)
    {
        if (Status != ProcessingStatus.Pending && Status != ProcessingStatus.RetryPending)
            throw new DomainException($"Cannot process message in {Status} status");

        Status = ProcessingStatus.Processing;
        SetLastProcessedAt(startedAt);
    }
    
    public void Complete(DateTimeOffset completedAt)
    {
        if (Status != ProcessingStatus.Processing)
            throw new DomainException($"Cannot complete message in {Status} status");

        Status = ProcessingStatus.Completed;
        SetLastProcessedAt(completedAt);
    }
    
    public void RecordFailure(Error error, DateTimeOffset occuredAt)
    {
        if (Status is ProcessingStatus.Completed or ProcessingStatus.DeadLettered)
            throw new DomainException($"Cannot record failure for message in {Status} status");

        var failure = ProcessingFailure.Create(error, occuredAt, Id);
        _failures.Add(failure);
        IncrementRetryCount();

        if (ShouldDeadLetter())
        {
            DeadLetter(failure, occuredAt);
        }
        else
        {
            ScheduleRetry();
        }
    }
    
    private void ScheduleRetry()
    {
        Status = ProcessingStatus.RetryPending;
        CalculateNextRetry();
        
        DomainException.ThrowIfNull(Metadata.NextRetryAt);
        RaiseEvent(new MessageRetryScheduled(Id, Metadata.NextRetryAt.Value));
    }

    private bool ShouldDeadLetter() => RetryPolicy.ShouldDeadLetter(Metadata.RetryCount);
    
    private void DeadLetter(ProcessingFailure finalProcessingFailure, DateTimeOffset occuredAt)
    {
        Status = ProcessingStatus.DeadLettered;
        DeadLetterEntry = DeadLetterEntry.Create(Id, finalProcessingFailure, occuredAt);
        
        RaiseEvent(new MessageDeadLettered(Id, DeadLetterEntry.Id));
    }

    private void SetLastProcessedAt(DateTimeOffset lastProcessedAt) => Metadata.SetLastProcessedAt(lastProcessedAt);
    private void IncrementRetryCount() => Metadata.IncrementRetryCount();
    private void CalculateNextRetry()
    {
        var nextRetryTime = RetryPolicy.CalculateNextRetryTime(Metadata.RetryCount);
        Metadata.SetNextRetryAt(nextRetryTime);
    }
}