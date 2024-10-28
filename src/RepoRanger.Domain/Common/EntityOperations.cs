using RepoRanger.Domain.Events;

namespace RepoRanger.Domain.Common;

public static class EntityOperations
{
    public static IReadOnlyCollection<DomainEvent> ExtractEventsForPublishing(this List<BaseEntity> entities)
    {
        var events = entities
            .SelectMany(e => e.GetEvents()).ToList();
    
        entities.ForEach(e => e.ClearEvents());

        return events;
    }
}