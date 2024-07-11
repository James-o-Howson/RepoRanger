using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Application.Events;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Persistence.Interceptors;

public class EventDispatcherSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IEventService _eventService;

    public EventDispatcherSaveChangesInterceptor(IEventService eventService)
    {
        _eventService = eventService;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is null) return base.SavedChanges(eventData, result);

        DispatchEventsAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();

        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

        await DispatchEventsAsync(eventData.Context, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
    
    private async Task DispatchEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var events = ExtractDomainEvents(context.ChangeTracker);
        if (events.Count == 0) return;

        foreach (var @event in events)
        {
            await _eventService.DispatchEventAsync(@event, cancellationToken);
        }
    }
    
    private static IReadOnlyCollection<IEvent> ExtractDomainEvents(ChangeTracker changeTracker)
    {
        var entitiesWithEvents = changeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.GetEvents().Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entitiesWithEvents
            .SelectMany(e => e.GetEvents()).ToList();
    
        entitiesWithEvents.ForEach(e => e.ClearEvents());
    
        return events;
    }
}