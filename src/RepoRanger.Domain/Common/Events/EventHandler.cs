namespace RepoRanger.Domain.Common.Events;

public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : IEvent
{
    public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}