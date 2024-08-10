using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Common;

public static class EntityOperations
{
    public static IReadOnlyCollection<IEvent> ExtractEventsForPublishing(this List<BaseEntity> entities)
    {
        var events = entities
            .SelectMany(e => e.GetEvents()).ToList();
    
        entities.ForEach(e => e.ClearEvents());

        return events;
    }
}