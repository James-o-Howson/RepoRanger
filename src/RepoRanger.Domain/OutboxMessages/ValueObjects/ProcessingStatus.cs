namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public enum ProcessingStatus
{
    Unprocessed = 1,
    Succeeded = 2,
    Failed = 3
}