﻿using System.Text.Json;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Domain.Messages;

public enum MessageStatus
{
    None = 0,
    Succeeded = 1,
    Failed = 2,
}

public class Message : Entity
{
    private Message()
    {
    }

    public static Message Create(IEvent @event) => new()
    {
        Data = JsonSerializer.Serialize(@event),
        Name = @event.GetType().Name
    };
    
    public Guid Id { get; } = Guid.NewGuid();
    public string Data { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int RetryCount { get; private set; }
    public bool Processed { get; private set; }
    public DateTime? ProcessStartTime { get; private set; }
    public DateTime? ProcessFinishedTime { get; private set; }
    public MessageStatus Status { get; private set; } = MessageStatus.None;
    public string? LastErrorDetails { get; private set; }
    
    public void Start()
    {
        ProcessStartTime = DateTime.Now;
    }

    public void Fail(Exception exception)
    {
        Processed = false;
        LastErrorDetails = exception.ToString();
        Status = MessageStatus.Failed;
    }

    public void Succeed(DateTime time)
    {
        Processed = true;
        ProcessFinishedTime = time;
        Status = MessageStatus.Succeeded;
    }

    public void Retry(int retryThreshold)
    {
        RetryCount++;
        ProcessStartTime = DateTime.Now;
        ProcessFinishedTime = null;

        if (RetryCount < retryThreshold)
        {
            return;
        }

        Fail(new MessageRetryThresholdExceededException(RetryCount, retryThreshold));
    }
}