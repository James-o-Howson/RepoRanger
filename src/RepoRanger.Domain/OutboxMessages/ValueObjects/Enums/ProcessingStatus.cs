namespace RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;

public enum ProcessingStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    DeadLettered = 4,
    RetryPending = 5
}