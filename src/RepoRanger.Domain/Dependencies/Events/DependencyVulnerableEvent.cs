using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Events;

public sealed class DependencyVulnerableEvent : Event
{
    // ReSharper disable once UnusedMember.Global
    public DependencyVulnerableEvent() { }
    
    public DependencyVulnerableEvent(VulnerabilityId vulnerabilityId) : 
        base(DateTimeOffset.UtcNow, EventType.Durable)
    {
        VulnerabilityId = vulnerabilityId;
    }
    
    public VulnerabilityId VulnerabilityId { get; set; }
    public override Type GetImplementationType() => typeof(DependencyVulnerableEvent);
}