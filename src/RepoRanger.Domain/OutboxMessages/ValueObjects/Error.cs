using RepoRanger.Domain.Common;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public class Error : ValueObject
{
    public string Message { get; init; } = string.Empty;
    public required ErrorSeverity Severity { get; init; }

    // ReSharper disable once UnusedMember.Local
    private Error() { }

    public static Error CreateInstance(Exception exception, ErrorSeverity severity) => new()
    {
        Message = exception.ToString(),
        Severity = severity
    };

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Message;
        yield return Severity;
    }
}