using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Events;

public sealed class RepositoryDeletedEvent : Event
{
    // ReSharper disable once UnusedMember.Global
    public RepositoryDeletedEvent() { }
    
    public RepositoryDeletedEvent(RepositoryId repositoryId) : 
        base(DateTimeOffset.UtcNow, EventType.Durable)
    {
        RepositoryId = repositoryId;
    }

    public RepositoryId RepositoryId { get; set; }
    
    public override Type GetImplementationType() => GetType();
}