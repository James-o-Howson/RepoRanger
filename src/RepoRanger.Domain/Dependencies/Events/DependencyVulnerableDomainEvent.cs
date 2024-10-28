using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Domain.Events;

namespace RepoRanger.Domain.Dependencies.Events;

public sealed class DependencyVulnerableDomainEvent : DomainEvent
{
    public DependencyVulnerableDomainEvent(VulnerabilityId vulnerabilityId) : 
        base(DateTimeOffset.UtcNow)
    {
        VulnerabilityId = vulnerabilityId;
    }
    
    public VulnerabilityId VulnerabilityId { get; set; }
}