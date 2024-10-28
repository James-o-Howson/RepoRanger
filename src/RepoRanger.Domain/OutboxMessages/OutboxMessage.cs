using RepoRanger.Domain.Common;
using RepoRanger.Domain.Events;
using RepoRanger.Domain.OutboxMessages.ValueObjects;

namespace RepoRanger.Domain.OutboxMessages;

public class OutboxMessage : BaseEntity
{
    private OutboxMessage() { }
    
    public static OutboxMessage Create(IntegrationEvent integrationEvent, DateTimeOffset created) => new()
    {
        Data = OutboxMessageData.From(integrationEvent),
        EventType = EventType.From(integrationEvent),
        Created = created,
    };
    
    public OutboxMessageId Id { get; } = OutboxMessageId.New;
    public required OutboxMessageData Data { get; init; }
    public required EventType EventType { get; init; }
    public int RetryCount { get; private set; }
    public DateTimeOffset? ProcessStartTime { get; private set; }
    public DateTimeOffset? ProcessFinishedTime { get; private set; }
    public required DateTimeOffset Created { get; init; }
    public ProcessingStatus ProcessingStatus { get; private set; } = ProcessingStatus.Unprocessed;
    public string? LastErrorDetails { get; private set; }

    public IntegrationEvent Event => Data.ToIntegrationEvent(EventType);

    public void StartProcessing(DateTimeOffset time)
    {
        if (RetryCount > 0 && ProcessStartTime is not null) return;
        
        ProcessStartTime = time;
    }

    public void Succeed(DateTimeOffset timeFinished)
    {
        ProcessFinishedTime = timeFinished;
        ProcessingStatus = ProcessingStatus.Succeeded;
    }

    public void Fail(int retryThreshold, Exception exception)
    {
        RetryCount++;
        ProcessFinishedTime = null;
        LastErrorDetails = exception.ToString();

        if (RetryCount < retryThreshold)
        {
            return;
        }

        ProcessingStatus = ProcessingStatus.Failed;
    }
}