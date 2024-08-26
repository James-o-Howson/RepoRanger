using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.PersistedEvents;

namespace RepoRanger.Data.Interceptors;

public class PersistEventsSaveChangesInterceptor : SaveChangesInterceptor
{
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
    
    private static async Task DispatchEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var events = context.ChangeTracker
            .Entries<BaseEntity>()
            .GetEntitiesWithEvents()
            .ExtractEventsForPublishing();
        
        if (events.Count == 0) return;

        var persistedEvents = events.Select(PersistedEvent.Create);

        await context.AddRangeAsync(persistedEvents, cancellationToken);
    }
}