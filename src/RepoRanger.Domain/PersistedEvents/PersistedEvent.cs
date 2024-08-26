using System.Text.Json;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.PersistedEvents.ValueObjects;

namespace RepoRanger.Domain.PersistedEvents;

public class PersistedEvent : BaseAuditableEntity
{
    private PersistedEvent() { }

    public static PersistedEvent Create(IEvent @event) => new()
    {
        Data = JsonSerializer.Serialize(@event),
        Name = @event.GetType().Name
    };
    
    public PersistedEventId Id { get; } = PersistedEventId.New;
    public string Data { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int RetryCount { get; private set; }
    public bool Processed { get; private set; }
    public DateTimeOffset? ProcessStartTime { get; private set; }
    public DateTimeOffset? ProcessFinishedTime { get; private set; }
    public PersistedEventStatus Status { get; private set; } = PersistedEventStatus.Unprocessed;
    public string? LastErrorDetails { get; private set; }

    public void StartProcessing(DateTimeOffset time)
    {
        if (RetryCount > 0 && ProcessStartTime is not null) return;
        
        ProcessStartTime = time;
    }

    public void Succeed(DateTimeOffset time)
    {
        Processed = true;
        ProcessFinishedTime = time;
        Status = PersistedEventStatus.Succeeded;
    }

    public void Retry(int retryThreshold, Exception exception)
    {
        RetryCount++;
        ProcessFinishedTime = null;

        if (RetryCount < retryThreshold)
        {
            return;
        }

        Fail(exception);
    }

    private void Fail(Exception exception)
    {
        Processed = false;
        LastErrorDetails = exception.ToString();
        Status = PersistedEventStatus.Failed;
    }
}