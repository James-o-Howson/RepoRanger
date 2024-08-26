namespace RepoRanger.Domain.PersistedEvents.ValueObjects;

public enum PersistedEventStatus
{
    Unprocessed = 0,
    Succeeded = 1,
    Failed = 2,
}