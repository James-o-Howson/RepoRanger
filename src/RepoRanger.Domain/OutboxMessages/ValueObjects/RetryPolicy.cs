using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public sealed class RetryPolicy : ValueObject
{
    public required int MaxRetries { get; init; }
    public required TimeSpan InitialDelay { get; init; }
    public required double BackoffMultiplier { get; init; }
    public required TimeSpan MaxDelay { get; init; }
    
    // ReSharper disable once UnusedMember.Local
    private RetryPolicy() { }

    public static RetryPolicy Default => new()
    {
        MaxRetries = 5,
        InitialDelay = TimeSpan.FromSeconds(1),
        BackoffMultiplier = 2,
        MaxDelay = TimeSpan.FromMinutes(10)
    };

    public bool ShouldDeadLetter(int retryCount) => retryCount >= MaxRetries;

    public DateTimeOffset CalculateNextRetryTime(int retryCount)
    {
        var delay = InitialDelay.TotalSeconds * Math.Pow(BackoffMultiplier, retryCount - 1);
        var cappedDelay = Math.Min(delay, MaxDelay.TotalSeconds);
        return DateTimeOffset.UtcNow.AddSeconds(cappedDelay);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MaxRetries;
        yield return InitialDelay;
        yield return BackoffMultiplier;
        yield return MaxDelay;
    }
}