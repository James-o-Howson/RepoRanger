namespace RepoRanger.Domain.PersistedEvents;

public interface IPersistedEventDispatcher
{
    Task DispatchAsync(PersistedEvent @event, CancellationToken cancellationToken = default);
}