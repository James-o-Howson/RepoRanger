using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Domain.Events;
using RepoRanger.Domain.OutboxMessages;

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