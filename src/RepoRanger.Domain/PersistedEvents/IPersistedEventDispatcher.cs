namespace RepoRanger.Domain.PersistedEvents;

public interface IPersistedEventDispatcher
{
    Task DispatchAsync(PersistedEvent persistedEvent, CancellationToken cancellationToken = default);
}