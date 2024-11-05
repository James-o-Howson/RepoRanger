using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public sealed class ProcessingMetadata : ValueObject
{
    public int RetryCount { get; private set; }
    public DateTimeOffset? LastProcessedAt { get; private set; }
    public DateTimeOffset? NextRetryAt { get; private set; }
    
    // ReSharper disable once UnusedMember.Local
    private ProcessingMetadata() { }

    public static ProcessingMetadata Initial => new()
    {
        RetryCount = 0,
        LastProcessedAt = null,
        NextRetryAt = null
    };
    
    public void IncrementRetryCount() => RetryCount++;
    public void SetNextRetryAt(DateTimeOffset nextRetryAt) => NextRetryAt = nextRetryAt;
    public void SetLastProcessedAt(DateTimeOffset lastProcessedAt) => LastProcessedAt = lastProcessedAt;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RetryCount;
        yield return LastProcessedAt;
        yield return NextRetryAt;
    }
}