using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Common;

public static class EntityOperations
{
    public static IReadOnlyCollection<IEvent> ExtractEvents(this List<Entity> entities)
    {
        var events = entities
            .SelectMany(e => e.GetEvents()).ToList();
    
        entities.ForEach(e => e.ClearEvents());

        return events;
    }
}