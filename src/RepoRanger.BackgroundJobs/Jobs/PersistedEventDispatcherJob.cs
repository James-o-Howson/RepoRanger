using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.BackgroundJobs.Abstractions;
using RepoRanger.BackgroundJobs.Abstractions.Options;
using RepoRanger.Domain.PersistedEvents;
using RepoRanger.Domain.PersistedEvents.ValueObjects;

namespace RepoRanger.BackgroundJobs.Jobs;

internal sealed class PersistedEventDispatcherJob : BaseJob<PersistedEventDispatcherJob>
{
    internal static readonly JobKey JobKey = new(nameof(PersistedEventDispatcherJob));

    private readonly IApplicationDbContext _dbContext;
    private readonly IPersistedEventDispatcher _persistedEventDispatcher;

    public PersistedEventDispatcherJob(ILogger<PersistedEventDispatcherJob> logger,
        IOptions<BackgroundJobOptions> options,
        IApplicationDbContext dbContext, IPersistedEventDispatcher persistedEventDispatcher) : base(logger,
        options)
    {
        _dbContext = dbContext;
        _persistedEventDispatcher = persistedEventDispatcher;
    }

    protected override async Task ExecuteJobLogicAsync(IJobExecutionContext context) => 
        await ProcessPersistedEventsAsync(context);

    private async Task ProcessPersistedEventsAsync(IJobExecutionContext context)
    {
        var unprocessedEvents = _dbContext.PersistedEvents
            .Where(e => e.Status == PersistedEventStatus.Unprocessed)
            .AsAsyncEnumerable();
        
        await foreach (var @event in unprocessedEvents)
        {
            await _persistedEventDispatcher.DispatchAsync(@event, context.CancellationToken);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
        
    }
    
    
}