using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Events;

public sealed class DependencyVulnerableEvent : Event
{
    public DependencyVulnerableEvent(VulnerabilityId vulnerabilityId, DependencyVersionId versionId, DependencySourceId dependencySourceId) : 
        base(DateTimeOffset.UtcNow, EventType.Durable)
    {
        VulnerabilityId = vulnerabilityId;
        VersionId = versionId;
        DependencySourceId = dependencySourceId;
    }
    
    public VulnerabilityId VulnerabilityId { get; }
    public DependencyVersionId VersionId { get; }
    public DependencySourceId DependencySourceId { get; }
}