using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Application.Sources.Parsing;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepoRangerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(RepoRangerJob));
    
    private readonly IGitParserService _gitParserService;
    private readonly ILogger<RepoRangerJob> _logger;
    private readonly QuartzOptions _quartzOptions;
    private readonly IMediator _mediator;

    public RepoRangerJob(ILogger<RepoRangerJob> logger,
        IOptions<QuartzOptions> options,
        IMediator mediator, 
        IGitParserService gitParserService)
    {
        _logger = logger;
        _mediator = mediator;
        _gitParserService = gitParserService;
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
        var sources = await _gitParserService.ParseAsync();

        foreach (var source in sources)
        {
            _logger.LogInformation("Repo Ranger Job Finished - Source created Id: {SourceId}", source.Id);
        }
    }
}