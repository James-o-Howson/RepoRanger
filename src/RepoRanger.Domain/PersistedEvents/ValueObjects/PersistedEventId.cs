namespace RepoRanger.Domain.PersistedEvents.ValueObjects;

public readonly record struct PersistedEventId(Guid Value)
{
    internal static PersistedEventId Empty => new(Guid.Empty);
    internal static PersistedEventId New => new(Guid.NewGuid());
};