﻿using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Repositories.Commands.CreateRepository;
using RepoRanger.Application.Sources.Commands.CreateSourceCommand;
using RepoRanger.Application.Sources.Commands.UpdateSourceCommand;
using RepoRanger.Application.Sources.Queries.GetByName;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Sources;
using RepoRanger.Domain.Sources.Repositories;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class SourceParserService : ISourceParserService
{
    private readonly ParsingContext _context;
    
    private readonly IMediator _mediator;
    private readonly SourceParserOptions _options;
    private readonly ILogger<SourceParserService> _logger;
    private readonly IGitRepositoryParser _gitRepositoryParser;

    private IEnumerable<SourceOptions> EnabledSourceOptions => _options.Sources.Where(s => s.Enabled);

    public SourceParserService(IEnumerable<ISourceFileParser> fileContentParsers,
        IOptions<SourceParserOptions> options,
        ILogger<SourceParserService> logger, 
        IMediator mediator, IGitRepositoryParser gitRepositoryParser)
    {
        ConcurrentBag<ISourceFileParser> sourceFileParsers = new(fileContentParsers);
        _logger = logger;
        _mediator = mediator;
        _gitRepositoryParser = gitRepositoryParser;
        _options = options.Value;

        _context = ParsingContext.Create(sourceFileParsers);
    }

    public async Task<IEnumerable<Source>> ParseAsync()
    {
        var parseTasks = EnabledSourceOptions.Select(ParseSourceAsync);
        return await Task.WhenAll(parseTasks);
    }

    private async Task<Source> ParseSourceAsync(SourceOptions sourceOptions)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);
        
        var source = await CreateOrUpdateSourceAsync(sourceOptions.Name, sourceOptions.Location);
        
        _context.StartParsing(source.Id);
        await ParseRepositoriesAsync(sourceOptions);
        _context.StopParsing();
        
        _logger.LogInformation("Finished Parsing Source {SourceName}", sourceOptions.Name);
        return source;
    }
    
    private async Task<Source> CreateOrUpdateSourceAsync(string name, string location)
    {
        var existing = await _mediator.Send(new GetSourceByNameQuery
        {
            Name = name
        });

        Guid id;
        if (existing is null)
        {
            id = await _mediator.Send(new CreateSourceCommand
            {
                Name = name,
                Location = location
            });

        }
        else
        {
            id = await _mediator.Send(new UpdateSourceCommand
            {
                Id = existing.Id,
                Location = existing.Location
            });
        }
        
        _logger.LogInformation("Repo Ranger Job Finished - Source created Id: {SourceId}", id);

        return Source.CreateExisting(id, name, location);
    }

    private async Task ParseRepositoriesAsync(SourceOptions sourceOptions)
    {
        var paths = sourceOptions.LocationInfo.GetGitDirectories();
        
        var repositories = await Task.WhenAll(paths
            .Where(p => !sourceOptions.IsExcluded(p))
            .Select(ParseRepositoryAsync));

        foreach (var repository in repositories)
        {
            await CreateOrUpdateRepository(repository);
        }
    }
    
    private async Task<Repository> ParseRepositoryAsync(string gitRepositoryPath)
    {
        var gitRepository = new DirectoryInfo(gitRepositoryPath);
        _context.GitDirectory = gitRepository;
        
        _logger.LogInformation("Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        var repository = await _gitRepositoryParser.ParseAsync(_context);
        
        _context.EnsureParsingContextValid();
        
        
        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        return repository;
    }

    private async Task CreateOrUpdateRepository(Repository repository)
    {
        await _mediator.Send(new CreateRepositoryCommand
        {
            Name = repository.Name,
            RemoteUrl = repository.RemoteUrl,
            BranchName = repository.DefaultBranch,
            SourceId = _context.SourceId!.Value
        });
    }
}