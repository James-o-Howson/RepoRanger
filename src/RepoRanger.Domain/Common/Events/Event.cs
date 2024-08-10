using MediatR;

namespace RepoRanger.Domain.Common.Events;

public interface IEvent : INotification
{
    DateTimeOffset OccuredOn { get; }
    EventType Type { get; }
}

public class Event : IEvent
{
    public DateTimeOffset OccuredOn { get; }
    public EventType Type { get; }

    protected Event(DateTimeOffset occuredOn, EventType type)
    {
        OccuredOn = occuredOn;
        Type = type;
    }
}