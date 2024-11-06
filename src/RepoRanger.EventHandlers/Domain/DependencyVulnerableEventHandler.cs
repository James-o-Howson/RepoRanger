using RepoRanger.Abstractions.Events.Domain;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Contracts.Vulnerabilities.IntegrationEvents;
using RepoRanger.Domain.Dependencies.Events;

namespace RepoRanger.EventHandlers.Domain;

internal sealed class DependencyVulnerableEventHandler : DomainEventHandler<DependencyVulnerabilityDiscovered>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DependencyVulnerableEventHandler(IIntegrationEventPublisher integrationEventPublisher)
    {
        _integrationEventPublisher = integrationEventPublisher;
    }

    protected override async Task HandleAsync(DependencyVulnerabilityDiscovered domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new VulnerabilityDiscoveredIntegrationEvent
        {
            VulnerabilityId = domainEvent.VulnerabilityId
        };
        
        await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
    }
}