using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Abstractions.Interfaces.Persistence;
using RepoRanger.BackgroundJobs.Abstractions;
using RepoRanger.BackgroundJobs.Abstractions.Options;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.ValueObjects;

namespace RepoRanger.BackgroundJobs.Jobs;

internal sealed class OutboxMessageProcessorJob : BaseJob<OutboxMessageProcessorJob>
{
    internal static readonly JobKey JobKey = new(nameof(OutboxMessageProcessorJob));

    private readonly IApplicationDbContext _dbContext;
    private readonly IOutboxMessageProcessor _outboxMessageProcessor;

    public OutboxMessageProcessorJob(ILogger<OutboxMessageProcessorJob> logger,
        IOptions<BackgroundJobOptions> options,
        IApplicationDbContext dbContext, IOutboxMessageProcessor outboxMessageProcessor) : base(logger,
        options)
    {
        _dbContext = dbContext;
        _outboxMessageProcessor = outboxMessageProcessor;
    }

    protected override async Task ExecuteJobLogicAsync(IJobExecutionContext context) => 
        await ProcessOutboxMessagesAsync(context);

    private async Task ProcessOutboxMessagesAsync(IJobExecutionContext context)
    {
        var unprocessedEvents = _dbContext.OutboxMessages
            .Where(e => e.ProcessingStatus == ProcessingStatus.Unprocessed)
            .AsAsyncEnumerable();
        
        await foreach (var @event in unprocessedEvents)
        {
            await _outboxMessageProcessor.DispatchAsync(@event, context.CancellationToken);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}