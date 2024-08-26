namespace RepoRanger.Domain.PersistedEvents;

internal sealed class PersistedEventRetryThresholdExceededException(int retryCount, int threshold)
    : Exception($"Retry count of \"{retryCount}\" exceeds threshold of \"{threshold}\".");
    