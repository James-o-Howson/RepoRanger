using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Events;

public class SourceCreatedEvent : Event
{
    public int SourceId { get; }

    public SourceCreatedEvent(int sourceId, DateTime occuredOn) 
        : base(occuredOn, EventType.Durable)
    {
        SourceId = sourceId;
    }
}