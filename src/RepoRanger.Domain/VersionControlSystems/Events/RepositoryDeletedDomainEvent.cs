using RepoRanger.Domain.Events;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Events;

public sealed class RepositoryDeletedDomainEvent : DomainEvent
{
    public RepositoryDeletedDomainEvent(RepositoryId repositoryId) : 
        base(DateTimeOffset.UtcNow)
    {
        RepositoryId = repositoryId;
    }

    public RepositoryId RepositoryId { get; set; }
}