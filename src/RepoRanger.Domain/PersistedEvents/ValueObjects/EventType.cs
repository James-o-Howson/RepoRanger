using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;

namespace RepoRanger.Domain.PersistedEvents.ValueObjects;

public sealed class EventType : ValueObject
{
    // ReSharper disable once UnusedMember.Local
    private EventType() { }

    private EventType(Type type) =>
        Value = type.FullName ?? 
                throw new DomainException("Error creating event, unable to get type for event");
    
    public string Value { get; init; } = null!;
    
    public static EventType From(Type type) =>
        new(type);
    
    public static implicit operator Type(EventType eventType) => CreateType(eventType.Value);
    public static explicit operator EventType(Type type) => From(type);
    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static Type CreateType(string value) =>
        Type.GetType(value) ?? 
        throw new DomainException($"Error converting event type full name {value} to type");
}