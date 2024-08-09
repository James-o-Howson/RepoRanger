using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Events;

internal sealed class DependencyVulnerableEvent : IEvent
{
    public DependencyId Id { get; }

    public DependencyVulnerableEvent(DependencyId id)
    {
        Id = id;
        OccuredOn = DateTime.Now;
    }

    public DateTime OccuredOn { get; }
    public EventType Type => EventType.Durable;
}