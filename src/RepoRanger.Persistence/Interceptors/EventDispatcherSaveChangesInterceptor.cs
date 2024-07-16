﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Application.Events;
using RepoRanger.Domain.Common;

namespace RepoRanger.Persistence.Interceptors;

public class EventDispatcherSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IEventDispatcher _eventDispatcher;

    public EventDispatcherSaveChangesInterceptor(IEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
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
        var events = context.ChangeTracker
            .Entries<Entity>()
            .WithEvents()
            .ExtractEvents();
        
        if (events.Count == 0) return;

        foreach (var @event in events)
        {
            await _eventDispatcher.DispatchEventAsync(@event, cancellationToken);
        }
    }
}