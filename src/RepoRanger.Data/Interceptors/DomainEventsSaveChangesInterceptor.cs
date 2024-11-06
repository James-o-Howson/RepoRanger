using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Abstractions.Events.Domain;
using SharedKernel.Base;

namespace RepoRanger.Data.Interceptors;

public class DomainEventsSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DomainEventsSaveChangesInterceptor(IDomainEventDispatcher domainEventDispatcher)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);

        DispatchEventsAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        await DispatchEventsAsync(eventData.Context, cancellationToken);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private async Task DispatchEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .GetEntitiesWithEvents();
        
        var events = entities
            .SelectMany(e => e.GetEvents()).ToList();
    
        entities.ForEach(e => e.ClearEvents());
        
        if (entities.Count == 0) return;
        
        await _domainEventDispatcher.DispatchAsync(events, cancellationToken);
    }
}