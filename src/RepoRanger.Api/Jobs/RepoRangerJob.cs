using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepoRangerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(RepoRangerJob));
    
    private readonly IVersionControlSystemParserService _versionControlSystemParserService;
    private readonly ILogger<RepoRangerJob> _logger;
    private readonly QuartzOptions _quartzOptions;

    public RepoRangerJob(ILogger<RepoRangerJob> logger,
        IOptions<QuartzOptions> options, 
        IVersionControlSystemParserService versionControlSystemParserService)
    {
        _logger = logger;
        _versionControlSystemParserService = versionControlSystemParserService;
        _quartzOptions = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Repo Ranger Job Starting");

            if (!_quartzOptions.RepoClonerJobEnabled)
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