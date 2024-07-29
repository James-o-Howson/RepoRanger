namespace RepoRanger.Domain.Messages;

internal sealed class MessageRetryThresholdExceededException(int retryCount, int threshold)
    : Exception($"Retry count of \"{retryCount}\" exceeds threshold of \"{threshold}\".");
    