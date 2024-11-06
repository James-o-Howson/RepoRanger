using MediatR;
using SharedKernel.Events.Integration;

namespace RepoRanger.Abstractions.Events.Integration;

public sealed class IntegrationEventNotification<TIntegrationEvent> : INotification
    where TIntegrationEvent : IIntegrationEvent
{
    public TIntegrationEvent IntegrationEvent { get; }

    public IntegrationEventNotification(TIntegrationEvent integrationEvent) => IntegrationEvent = integrationEvent;
}