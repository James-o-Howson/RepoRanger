using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Domain.VersionControlSystems.Parsing;

namespace RepoRanger.BackgroundJobs.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepoRangerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(RepoRangerJob));
    
    private readonly IVersionControlSystemParserService _versionControlSystemParserService;
    private readonly ILogger<RepoRangerJob> _logger;
    private readonly BackgroundJobOptions _backgroundJobOptions;

    public RepoRangerJob(ILogger<RepoRangerJob> logger,
        IOptions<BackgroundJobOptions> options, 
        IVersionControlSystemParserService versionControlSystemParserService)
    {
        _logger = logger;
        _versionControlSystemParserService = versionControlSystemParserService;
        _backgroundJobOptions = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Repo Ranger Job Starting");

            if (!_backgroundJobOptions.RepoClonerJobEnabled)
            {
                _logger.LogInformation("Repo Ranger Job Disabled - Skipping");
                return;
            }

            await StartRanging(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Repo Ranger Job - Error");
            context.Result = e;
        }
    }

    private async Task StartRanging(CancellationToken cancellationToken)
    {
        await _versionControlSystemParserService.ParseAsync(cancellationToken);
        _logger.LogInformation("Repo Ranger Job Finished");
    }
}