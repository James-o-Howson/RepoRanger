namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public record ProcessingMetadata(int RetryCount, DateTimeOffset? LastProcessedAt, DateTimeOffset? NextRetryAt)
{
    public ProcessingMetadata IncrementRetry() => 
        this with { RetryCount = RetryCount + 1 };

    public ProcessingMetadata WithNextRetry(DateTimeOffset nextRetry) =>
        this with { NextRetryAt = nextRetry };
    
    public ProcessingMetadata WithLastProcessedAt(DateTimeOffset lastProcessedAt) =>
        this with { LastProcessedAt = lastProcessedAt };

    public static ProcessingMetadata Initial => 
        new(RetryCount: 0, LastProcessedAt: null, NextRetryAt: null);
}