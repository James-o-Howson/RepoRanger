using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Domain.Common;

namespace RepoRanger.Data.Interceptors;

public class DomainEventsSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventsSaveChangesInterceptor(IMediator mediator)
    {
        _mediator = mediator;
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
        var events = context.ChangeTracker
            .Entries<BaseEntity>()
            .GetEntitiesWithEvents()
            .ExtractEventsForPublishing();
        
        if (events.Count == 0) return;

        foreach (var domainEvent in events)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}