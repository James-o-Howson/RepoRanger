using System.Text.Json;
using MediatR;

namespace RepoRanger.Domain.Common.Events;

public interface IEvent : INotification
{
    DateTimeOffset OccuredOn { get; }
    EventType Type { get; }
    Type GetImplementationType();
    string Serialize();
}

public abstract class Event : IEvent
{
    public DateTimeOffset OccuredOn { get; set; }
    public EventType Type { get; set; }
    
    protected Event() { }

    protected Event(DateTimeOffset occuredOn, EventType type)
    {
        OccuredOn = occuredOn;
        Type = type;
    }
    
    public string Serialize() => JsonSerializer.Serialize(this, GetImplementationType());

    public abstract Type GetImplementationType();
}