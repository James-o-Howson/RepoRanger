using RepoRanger.Domain.Events;

namespace RepoRanger.Domain.Common;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _events = [];
    
    protected BaseEntity()
    {
        // Required by EF Core.
    }
    
    public IReadOnlyCollection<DomainEvent> GetEvents() => _events.ToList();
    public void ClearEvents() => _events.Clear();
    protected void RaiseEvent(DomainEvent @event) => _events.Add(@event);
}