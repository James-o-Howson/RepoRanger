using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.OutboxMessages;
using SharedKernel.Events.Integration;

namespace RepoRanger.Infrastructure.Events;

internal sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly TimeProvider _timeProvider;

    public IntegrationEventPublisher(IApplicationDbContext applicationDbContext, TimeProvider timeProvider)
    {
        _applicationDbContext = applicationDbContext;
        _timeProvider = timeProvider;
    }

    public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var outboxMessage = OutboxMessage.Create(integrationEvent, _timeProvider.GetUtcNow());
        
        await _applicationDbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}