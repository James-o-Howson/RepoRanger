using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Domain.SourceParsing;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepoRangerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(RepoRangerJob));
    
    private readonly ISourceParserService _sourceParserService;
    private readonly ILogger<RepoRangerJob> _logger;
    private readonly QuartzOptions _quartzOptions;
    private readonly IMediator _mediator;

    public RepoRangerJob(ILogger<RepoRangerJob> logger,
        IOptions<QuartzOptions> options,
        IMediator mediator, 
        ISourceParserService sourceParserService)
    {
        _logger = logger;
        _mediator = mediator;
        _sourceParserService = sourceParserService;
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

            await StartRanging();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Repo Ranger Job - Error");
            context.Result = e;
        }
    }

    private async Task StartRanging()
    {
        await _sourceParserService.ParseAsync();
        _logger.LogInformation("Repo Ranger Job Finished");
    }
}