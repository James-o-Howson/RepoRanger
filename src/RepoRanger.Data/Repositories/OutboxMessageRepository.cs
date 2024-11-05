using Microsoft.EntityFrameworkCore;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;

namespace RepoRanger.Data.Repositories;

internal sealed class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly IApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;

    public OutboxMessageRepository(IApplicationDbContext context, TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow();
        
        return await _context.OutboxMessages
            .Where(m => 
                m.Status == ProcessingStatus.Pending ||
                (m.Status == ProcessingStatus.RetryPending && 
                 _timeProvider.GetUtcNow() <= now))
            .OrderBy(m => m.CreatedAt) 
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }
}