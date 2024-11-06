using RepoRanger.SharedKernel.Abstractions;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

public readonly record struct DeadLetterEntryId(Guid Value) : IId
{
    internal static DeadLetterEntryId Empty => new(Guid.Empty);
    internal static DeadLetterEntryId New => new(Guid.NewGuid());
}