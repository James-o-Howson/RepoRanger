using RepoRanger.SharedKernel.Abstractions;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

public readonly record struct OutboxMessageId(Guid Value) : IId
{
    internal static OutboxMessageId Empty => new(Guid.Empty);
    internal static OutboxMessageId New => new(Guid.NewGuid());
}