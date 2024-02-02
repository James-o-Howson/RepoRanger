using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Application.Sources.Commands.CreateSourceCommand;
using RepoRanger.Application.Sources.Commands.DeleteSourceCommand;
using RepoRanger.Application.Sources.Queries.GetByName;
using RepoRanger.Infrastructure.AzureDevOps;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class AzureDevOpsRepoRangerJob : IJob
{
    private readonly ILogger<AzureDevOpsRepoRangerJob> _logger;
    private readonly QuartzOptions _quartzOptions;
    private readonly IAzureDevOpsRepositoryDataExtractor _azureDevOpsRepositoryDataExtractor;
    private readonly IMediator _mediator;

    public AzureDevOpsRepoRangerJob(ILogger<AzureDevOpsRepoRangerJob> logger,
        IOptions<QuartzOptions> options,
        IAzureDevOpsRepositoryDataExtractor azureDevOpsRepositoryDataExtractor,
        IMediator mediator)
    {
        _logger = logger;
        _azureDevOpsRepositoryDataExtractor = azureDevOpsRepositoryDataExtractor;
        _mediator = mediator;
        _quartzOptions = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Azure Repo Cloner Job Starting");

            if (!_quartzOptions.RepoClonerJobEnabled)
            {
                _logger.LogInformation("Azure Repo Cloner Job Disabled - Skipping");
                return;
            }

            var sourceDto = await _azureDevOpsRepositoryDataExtractor.GetAzureRepositoriesAsync();
            await DeleteIfSourceExistsAsync(sourceDto);
            await CreateAsync(sourceDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Azure Repo Cloner Job - Error");
            context.Result = e;
        }
    }

    private async Task CreateAsync(SourceDto sourceDto)
    {
        var result = await _mediator.Send(new CreateSourceCommand(sourceDto.Name, sourceDto.Repositories));
        _logger.LogInformation("Azure Repo Cloner Job Finished - Source created Id: {SourceId}", result);
    }

    private async Task DeleteIfSourceExistsAsync(SourceDto sourceDto)
    {
        var existing = await _mediator.Send(new GetByNameQuery(sourceDto.Name));

        if (existing is not null)
        {
            await _mediator.Send(new DeleteSourceCommand(existing.Id));
        }
    }
}