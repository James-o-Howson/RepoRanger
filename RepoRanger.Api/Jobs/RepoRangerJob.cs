using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Application.Sources.Commands.CreateSourceCommand;
using RepoRanger.Application.Sources.Commands.DeleteSourceCommand;
using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Mapping;
using RepoRanger.Application.Sources.Queries.GetByName;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepoRangerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(RepoRangerJob));
    
    private readonly ISourceParser _sourceParser;
    private readonly ILogger<RepoRangerJob> _logger;
    private readonly QuartzOptions _quartzOptions;
    private readonly IMediator _mediator;

    public RepoRangerJob(ILogger<RepoRangerJob> logger,
        IOptions<QuartzOptions> options,
        IMediator mediator, 
        ISourceParser sourceParser)
    {
        _logger = logger;
        _mediator = mediator;
        _sourceParser = sourceParser;
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
        var sourceContexts = await _sourceParser.ParseAsync();
        foreach (var sourceDto in sourceContexts.ToDtos())
        {
            await DeleteIfSourceExistsAsync(sourceDto);
            await CreateAsync(sourceDto);
        }
    }

    private async Task CreateAsync(SourceDto sourceDto)
    {
        var result = await _mediator.Send(new CreateSourceCommand(sourceDto.Name, sourceDto.Repositories));
        _logger.LogInformation("Repo Ranger Job Finished - Source created Id: {SourceId}", result);
    }

    private async Task DeleteIfSourceExistsAsync(SourceDto sourceDto)
    {
        var existing = await _mediator.Send(new GetByNameQuery(sourceDto.Name));

        if (existing is not null)
        {
            await _mediator.Send(new DeleteSourceCommand
            {
                Id = existing.Id
            });
        }
    }
}