namespace RepoRanger.Domain.Common.Events;

public interface IEvent
{
    DateTime OccuredOn { get; }
    EventType Type { get; }
}