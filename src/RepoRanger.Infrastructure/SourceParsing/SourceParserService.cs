using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Sources.Commands.CreateSourceCommand;
using RepoRanger.Application.Sources.Commands.UpdateSourceCommand;
using RepoRanger.Application.Sources.Queries.GetByName;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class SourceParserService : ISourceParserService
{
    private readonly ParsingContext _context;

    private readonly ILogger<SourceParserService> _logger;

    private readonly IMediator _mediator;
    private readonly SourceParserOptions _options;
    private readonly IRepositoryParser _repositoryParser;

    public SourceParserService(IEnumerable<ISourceFileParser> fileContentParsers,
        IOptions<SourceParserOptions> options,
        ILogger<SourceParserService> logger,
        IMediator mediator, IRepositoryParser repositoryParser)
    {
        ConcurrentBag<ISourceFileParser> sourceFileParsers = new(fileContentParsers);
        _logger = logger;
        _mediator = mediator;
        _repositoryParser = repositoryParser;
        _options = options.Value;

        _context = ParsingContext.Create(sourceFileParsers);
    }

    private IEnumerable<SourceOptions> EnabledSourceOptions => 
        _options.Sources.Where(s => s.Enabled);

    public async Task ParseAsync() => 
        await Task.WhenAll(EnabledSourceOptions.Select(ParseSourceAsync));

    private async Task ParseSourceAsync(SourceOptions sourceOptions)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);

        Source source = await CreateOrUpdateSourceAsync(sourceOptions.Name, sourceOptions.Location);

        _context.StartParsing(source.Id);

        IEnumerable<Repository> repositories = await ParseRepositoriesAsync(sourceOptions);
        source.AddRepositories(repositories);

        _context.StopParsing();

        _logger.LogInformation("Finished Parsing Source {SourceName}:{Id}", sourceOptions.Name, source.Id);
    }

    private async Task<Source> CreateOrUpdateSourceAsync(string name, string location)
    {
        SourcePreviewDto? existing = await _mediator.Send(new GetSourceByNameQuery { Name = name });

        int id;
        if (existing != null)
        {
            id = await _mediator.Send(new CreateSourceCommand { Name = name, Location = location });
        }
        else
        {
            id = await _mediator.Send(new UpdateSourceCommand { Id = existing.Id, Location = existing.Location });
        }

        _logger.LogInformation("Repo Ranger Job Finished - Source created Id: {SourceId}", id);

        return Source.Create(name, location);
    }

    private async Task<IEnumerable<Repository>> ParseRepositoriesAsync(SourceOptions sourceOptions)
    {
        IEnumerable<string> paths = sourceOptions.LocationInfo.GetGitDirectories();

        return await Task.WhenAll(paths
            .Where(p => !sourceOptions.IsExcluded(p))
            .Select(ParseRepositoryAsync));
    }

    private async Task<Repository> ParseRepositoryAsync(string repositoryPath)
    {
        _context.GitDirectory = new DirectoryInfo(repositoryPath);

        _logger.LogInformation("Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        Repository repository = await _repositoryParser.ParseAsync(_context);

        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        return repository;
    }
}