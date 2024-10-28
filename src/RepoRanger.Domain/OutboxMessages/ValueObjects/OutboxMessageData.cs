using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RepoRanger.Domain.OutboxMessages.ValueObjects;

public sealed class OutboxMessageData : ValueObject
{
    private static JsonSerializerSettings DefaultOptions => new()
    {
        Formatting = Formatting.Indented
    };
    
    // ReSharper disable once UnusedMember.Local
    private OutboxMessageData() { }
    
    private OutboxMessageData(IntegrationEvent @event)
    {
        var type = EventType.From(@event);
        Value = JsonConvert.SerializeObject(@event, type, DefaultOptions);
    }
    
    public string Value { get; init; } = string.Empty;

    public override string ToString() => Value;
    
    public static OutboxMessageData From(IntegrationEvent @event) => new(@event);

    public IntegrationEvent ToIntegrationEvent(Type eventType)
    {
        var eventObject = JsonConvert.DeserializeObject(Value, eventType);
        if (eventObject is IntegrationEvent @event) return @event;
        
        throw new DomainException($"Unable to convert OutboxMessageData to IntegrationEvent of Type = {eventType}");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
}