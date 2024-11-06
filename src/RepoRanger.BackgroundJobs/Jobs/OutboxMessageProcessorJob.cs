using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.BackgroundJobs.Abstractions;
using RepoRanger.BackgroundJobs.Abstractions.Options;
using RepoRanger.Domain.OutboxMessages;

namespace RepoRanger.BackgroundJobs.Jobs;

internal sealed class OutboxMessageProcessorJob : BaseJob<OutboxMessageProcessorJob>
{
    internal static readonly JobKey JobKey = new(nameof(OutboxMessageProcessorJob));

    private readonly IOutboxMessageDispatcher _outboxMessageDispatcher;

    public OutboxMessageProcessorJob(ILogger<OutboxMessageProcessorJob> logger,
        IOptions<BackgroundJobOptions> backgroundJobOptions,
        IOutboxMessageDispatcher outboxMessageDispatcher) : base(logger,
        backgroundJobOptions)
    {
        _outboxMessageDispatcher = outboxMessageDispatcher;
    }

    protected override async Task ExecuteJobLogicAsync(IJobExecutionContext context) => 
        await _outboxMessageDispatcher.ProcessAsync(context.CancellationToken);
}