namespace RepoRanger.Domain.Common.Events;

public interface IEvent
{
    DateTime OccuredOn { get; }
    EventType Type { get; }
}

public class Event : IEvent
{
    public DateTime OccuredOn { get; }
    public EventType Type { get; }

    protected Event(DateTime occuredOn, EventType type)
    {
        OccuredOn = occuredOn;
        Type = type;
    }
}