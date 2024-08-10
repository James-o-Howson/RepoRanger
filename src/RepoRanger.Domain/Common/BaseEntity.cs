using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Common;

public abstract class BaseEntity
{
    private readonly List<IEvent> _events = [];
    
    protected BaseEntity()
    {
        // Required by EF Core.
    }
    
    public IReadOnlyCollection<IEvent> GetEvents() => _events.ToList();
    public void ClearEvents() => _events.Clear();
    protected void RaiseEvent(IEvent @event) => _events.Add(@event);
}