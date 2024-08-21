using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.BackgroundJobs.Abstractions;
using RepoRanger.BackgroundJobs.Abstractions.Options;
using RepoRanger.Domain.VersionControlSystems.Parsing;

namespace RepoRanger.BackgroundJobs.Jobs;

[DisallowConcurrentExecution]
internal sealed class VcsParserJob : BaseJob<VcsParserJob>
{
    internal static readonly JobKey JobKey = new(nameof(VcsParserJob));
    
    private readonly IVersionControlSystemParserService _versionControlSystemParserService;

    public VcsParserJob(ILogger<VcsParserJob> logger,
        IOptions<BackgroundJobOptions> options, 
        IVersionControlSystemParserService versionControlSystemParserService) : base(logger, options)
    {
        _versionControlSystemParserService = versionControlSystemParserService;
    }

    protected override async Task ExecuteJobLogicAsync(IJobExecutionContext context)
    {
        await _versionControlSystemParserService.ParseAsync(context.CancellationToken);
    }
}