using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.BackgroundJobs.Abstractions.Options;

namespace RepoRanger.BackgroundJobs.Abstractions;

internal abstract class BaseJob<TJob> : IJob
    where TJob : IJob
{
    private readonly ILogger<BaseJob<TJob>> _logger;
    private readonly BackgroundJobOptions _options;

    protected BaseJob(ILogger<BaseJob<TJob>> logger, IOptions<BackgroundJobOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        try
        {
            var jobKey = context.JobDetail.Key;
            if (!_options.IsEnabled(jobKey))
            {
                _logger.LogInformation("{JobName} - Skipping", jobKey.Name);
            }
            else
            {
                _logger.LogInformation("{CurrentJobName} Job Starting", jobKey.Name);
                await ExecuteJobLogicAsync(context);
                _logger.LogInformation("{CurrentJobName} Job Finished", jobKey.Name);
            }
            
            await TriggerNextJob(context);
        }
        catch (Exception e)
        {
            HandleException(e, context);
        }
    }

    protected abstract Task ExecuteJobLogicAsync(IJobExecutionContext context);

    protected virtual void HandleException(Exception e, IJobExecutionContext context)
    {
        var currentJobName = context.JobDetail.Key.Name;
        
        _logger.LogError(e, "{CurrentJobName} - Error", currentJobName);
        context.Result = e;
    }

    private async Task TriggerNextJob(IJobExecutionContext context)
    {
        var jobKey = context.JobDetail.Key;
        var nextJobKey = _options.NextJobKey(jobKey);
        if (nextJobKey is null || Equals(nextJobKey, jobKey)) return;

        var nextJob = await context.Scheduler.GetJobDetail(nextJobKey);
        if (nextJob is null) return;
            
        await context.Scheduler.TriggerJob(nextJobKey);
        _logger.LogInformation("Sequential Job Execution: Starting {NextJobName}", nextJob.Key.Name);
    }
}