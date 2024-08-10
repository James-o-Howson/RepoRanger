using System.Text.Json;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Messages;

public enum MessageStatus
{
    None = 0,
    Succeeded = 1,
    Failed = 2,
}

public class Message : BaseAuditableEntity
{
    private Message() { }

    public static Message Create(IEvent @event) => new()
    {
        Data = JsonSerializer.Serialize(@event),
        Name = @event.GetType().Name
    };
    
    public MessageId Id { get; } = MessageId.New;
    public string Data { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int RetryCount { get; private set; }
    public bool Processed { get; private set; }
    public DateTimeOffset? ProcessStartTime { get; private set; }
    public DateTimeOffset? ProcessFinishedTime { get; private set; }
    public MessageStatus Status { get; private set; } = MessageStatus.None;
    public string? LastErrorDetails { get; private set; }
    
    public void Start()
    {
        ProcessStartTime = DateTimeOffset.UtcNow;
    }

    public void Fail(Exception exception)
    {
        Processed = false;
        LastErrorDetails = exception.ToString();
        Status = MessageStatus.Failed;
    }

    public void Succeed(DateTimeOffset time)
    {
        Processed = true;
        ProcessFinishedTime = time;
        Status = MessageStatus.Succeeded;
    }

    public void Retry(int retryThreshold)
    {
        RetryCount++;
        ProcessStartTime = DateTimeOffset.UtcNow;
        ProcessFinishedTime = null;

        if (RetryCount < retryThreshold)
        {
            return;
        }

        Fail(new MessageRetryThresholdExceededException(RetryCount, retryThreshold));
    }
}