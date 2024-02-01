using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Application.Services;
using RepoRanger.Infrastructure.AzureDevOps;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepositoryDataCollectorJob : IJob
{
    private readonly ILogger<RepositoryDataCollectorJob> _logger;
    private readonly QuartzOptions _quartzOptions;
    private readonly IAzureDevOpsRepositoryDataExtractor _azureDevOpsRepositoryDataExtractor;
    private readonly IRepositoryService _repositoryService;

    public RepositoryDataCollectorJob(ILogger<RepositoryDataCollectorJob> logger, IOptions<QuartzOptions> options, IAzureDevOpsRepositoryDataExtractor azureDevOpsRepositoryDataExtractor, IRepositoryService repositoryService)
    {
        _logger = logger;
        _azureDevOpsRepositoryDataExtractor = azureDevOpsRepositoryDataExtractor;
        _repositoryService = repositoryService;
        _quartzOptions = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Repo Cloner Job - Starting");

            if (!_quartzOptions.RepoClonerJobEnabled)
            {
                _logger.LogInformation("Repo Cloner Job Disabled - Skipping");
                return;
            }

            var azureDevOpsRepositories = await _azureDevOpsRepositoryDataExtractor.GetAzureRepositoryDefinitionsAsync();
            await _repositoryService.SaveAsync(azureDevOpsRepositories, context.CancellationToken);

            _logger.LogInformation("Repo Cloner Job - Finished");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Repo Cloner Job - Error");
            context.Result = e;
        }
    }
}