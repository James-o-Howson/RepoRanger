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

    private readonly IOutboxMessageProcessor _outboxMessageProcessor;

    public OutboxMessageProcessorJob(ILogger<OutboxMessageProcessorJob> logger,
        IOptions<BackgroundJobOptions> backgroundJobOptions,
        IOutboxMessageProcessor outboxMessageProcessor) : base(logger,
        backgroundJobOptions)
    {
        _outboxMessageProcessor = outboxMessageProcessor;
    }

    protected override async Task ExecuteJobLogicAsync(IJobExecutionContext context) => 
        await _outboxMessageProcessor.ProcessAsync(context.CancellationToken);
}