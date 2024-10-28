using RepoRanger.Domain.Events;

namespace RepoRanger.Abstractions.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}