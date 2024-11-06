using RepoRanger.SharedKernel.Events.Integration;

namespace RepoRanger.Abstractions.Events.Integration;

public static class IntegrationEventMapper
{
    public static object ToNotification(this IIntegrationEvent integrationEvent)
    {
        var notificationType = typeof(IntegrationEventNotification<>)
            .MakeGenericType(integrationEvent.GetType());
            
        var notification = Activator.CreateInstance(notificationType, integrationEvent);
        ArgumentNullException.ThrowIfNull(notification);
        
        return notification;
    }
}