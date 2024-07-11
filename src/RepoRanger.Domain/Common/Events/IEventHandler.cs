namespace RepoRanger.Domain.Common.Events;

public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

public interface ITransientEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent { }
public interface IDurableEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent { }