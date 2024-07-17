using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Application.Events;

public interface IEventDispatcher
{
    Task DispatchEventAsync(IEvent @event, CancellationToken cancellationToken);
}

public class EventDispatcher : IEventDispatcher
{
    private readonly ITransientEventDispatcher _transientEventDispatcher;
    private readonly IDurableEventDispatcher _durableEventDispatcher;

    public EventDispatcher(ITransientEventDispatcher transientEventDispatcher)
    {
        _transientEventDispatcher = transientEventDispatcher;
        // _durableEventDispatcher = durableEventDispatcher;
    }
    
    public async Task DispatchEventAsync(IEvent @event, CancellationToken cancellationToken)
    {
        switch (@event.Type)
        {
            case EventType.Transient:
                await _transientEventDispatcher.DispatchAsync(@event, cancellationToken);
                break;
            case EventType.Durable:
                await _durableEventDispatcher.DispatchAsync(@event, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(@event.Type), "Unrecognised Event Type");
        }
    }
}