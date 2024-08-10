using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Events;

internal sealed class DependencyVulnerableEvent : IEvent
{
    public DependencyId Id { get; }

    public DependencyVulnerableEvent(DependencyId id)
    {
        Id = id;
        OccuredOn = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset OccuredOn { get; }
    public EventType Type => EventType.Durable;
}