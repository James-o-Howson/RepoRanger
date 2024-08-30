using System.Text.Json;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.PersistedEvents.ValueObjects;
using EventType = RepoRanger.Domain.PersistedEvents.ValueObjects.EventType;

namespace RepoRanger.Domain.PersistedEvents;

public class PersistedEvent : BaseEntity
{
    private PersistedEvent() { }

    public static PersistedEvent Create(IEvent @event, DateTimeOffset created) => new()
    {
        Data = @event.Serialize(),
        EventType = EventType.From(@event.GetType()),
        Created = created,
        Category = EventCategory.Domain
    };
    
    public PersistedEventId Id { get; } = PersistedEventId.New;
    public string Data { get; init; } = string.Empty;
    public EventType EventType { get; init; } = null!;
    public EventCategory Category { get; init; }
    public int RetryCount { get; private set; }
    public DateTimeOffset? ProcessStartTime { get; private set; }
    public DateTimeOffset? ProcessFinishedTime { get; private set; }
    public DateTimeOffset Created { get; init; }
    public PersistedEventStatus Status { get; private set; } = PersistedEventStatus.Unprocessed;
    public string? LastErrorDetails { get; private set; }
    
    public IEvent Event() =>
        JsonSerializer.Deserialize(Data, EventType) as IEvent ??
        throw new DomainException($"Event could not be deserialized for PersistedEvent with Id {Id}");

    public void StartProcessing(DateTimeOffset time)
    {
        if (RetryCount > 0 && ProcessStartTime is not null) return;
        
        ProcessStartTime = time;
    }

    public void Succeed(DateTimeOffset time)
    {
        ProcessFinishedTime = time;
        Status = PersistedEventStatus.Succeeded;
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

        Status = PersistedEventStatus.Failed;
    }
}