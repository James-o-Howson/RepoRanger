using MediatR;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies.Events;
using RepoRanger.EventHandlers.IntegrationEvents;

namespace RepoRanger.EventHandlers.Domain;

internal sealed class DependencyVulnerableEventHandler : INotificationHandler<DependencyVulnerableDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DependencyVulnerableEventHandler(IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(DependencyVulnerableDomainEvent notification, CancellationToken cancellationToken = default)
    {
        var integrationEvent = new VulnerabilityDiscoveredIntegrationEvent
        {
            VulnerabilityId = notification.VulnerabilityId
        };
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
    }
}