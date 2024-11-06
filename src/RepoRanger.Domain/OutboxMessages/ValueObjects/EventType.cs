using RepoRanger.Domain.Exceptions;
using RepoRanger.SharedKernel.Base;
using RepoRanger.SharedKernel.Events.Integration;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public sealed class EventType : ValueObject
{
    // ReSharper disable once UnusedMember.Local
    private EventType() { }

    private EventType(Type type)
    {
        Value = type.AssemblyQualifiedName ??
                throw new DomainException("Error creating event type value object, unable to get type for event");
    }

    public string Value { get; init; } = null!;
    
    public static EventType From(IntegrationEvent @event) =>
        From(CreateType(@event.AssemblyQualifiedName));
    
    public static EventType From(Type type) =>
        new(type);
    
    public static implicit operator Type(EventType eventType) => CreateType(eventType.ToString());
    public static explicit operator EventType(Type type) => From(type);
    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static Type CreateType(string value) =>
        Type.GetType(value) ?? 
        throw new DomainException($"Error converting integration event type full name {value} to type");
}