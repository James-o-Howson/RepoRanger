namespace RepoRanger.Domain.Messages;

public readonly record struct MessageId(Guid Value)
{
    internal static MessageId Empty => new(Guid.Empty);
    internal static MessageId New => new(Guid.NewGuid());
};