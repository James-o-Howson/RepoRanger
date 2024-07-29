using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Dependencies.Events;

internal sealed class DependencyVulnerableEvent : IEvent
{
    public Guid Id { get; }

    public DependencyVulnerableEvent(Guid id)
    {
        Id = id;
        OccuredOn = DateTime.Now;
    }

    public DateTime OccuredOn { get; }
    public EventType Type => EventType.Durable;
}