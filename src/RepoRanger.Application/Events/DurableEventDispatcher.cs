using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Common.Events;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Events;

public interface IDurableEventDispatcher
{
    Task DispatchAsync(IEvent @event, CancellationToken cancellationToken);
}

internal sealed class DurableEventDispatcher : IDurableEventDispatcher
{
    private readonly IApplicationDbContext _dbContext;

    public DurableEventDispatcher(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Type != EventType.Durable) throw new ArgumentException($"Event is not {nameof(EventType.Durable)}", nameof(@event));
            
        var message = Message.Create(@event);
        
        await _dbContext.Messages.AddAsync(message, cancellationToken);
    }
}