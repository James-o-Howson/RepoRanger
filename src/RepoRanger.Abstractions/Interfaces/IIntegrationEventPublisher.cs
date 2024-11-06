using RepoRanger.SharedKernel.Events.Integration;

namespace RepoRanger.Abstractions.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}