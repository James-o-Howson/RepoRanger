using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;

namespace RepoRanger.Domain.PersistedEvents.ValueObjects;

public sealed class EventTypeDescriptor : ValueObject
{
    // ReSharper disable once UnusedMember.Local
    private EventTypeDescriptor() { }

    private EventTypeDescriptor(Type type) =>
        Value = type.FullName ?? 
                throw new DomainException("Error creating event, unable to get type for event");
    
    public string Value { get; init; } = null!;
    
    public static EventTypeDescriptor From(Type type) =>
        new(type);
    
    public static implicit operator Type(EventTypeDescriptor eventTypeDescriptor) => CreateType(eventTypeDescriptor.Value);
    public static explicit operator EventTypeDescriptor(Type type) => From(type);
    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static Type CreateType(string value) =>
        Type.GetType(value) ?? 
        throw new DomainException($"Error converting event type full name {value} to type");
}