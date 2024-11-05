using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

public readonly record struct ProcessingFailureId(Guid Value) : IId
{
    internal static ProcessingFailureId Empty => new(Guid.Empty);
    internal static ProcessingFailureId New => new(Guid.NewGuid());
};