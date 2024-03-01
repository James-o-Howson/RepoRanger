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

internal sealed class SourceParserService : ISourceParserService, IDisposable
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

    public async Task ParseAsync()
    {
        var sources = await Task.WhenAll(EnabledSourceOptions.Select(ParseSourceAsync));

        foreach (var source in sources)
        {
            await CreateOrUpdate(source);
        }
    }

    private async Task CreateOrUpdate(Source source)
    {
        if (!source.HasId)
        {
            await _mediator.Send(new CreateSourceCommand
            {
                Name = source.Name,
                Location = source.Location
            });
        }
        else
        {
            await _mediator.Send(new UpdateSourceCommand
            {
                Id = source.Id,
                Location = source.Location
            });
        }
    }

    private async Task<Source> ParseSourceAsync(SourceOptions sourceOptions)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);

        var source = await GetSource(sourceOptions.Name, sourceOptions.Location);

        var repositories = await ParseRepositoriesAsync(sourceOptions);
        source.AddRepositories(repositories);

        _logger.LogInformation("Finished Parsing Source {SourceName}:{Id}", sourceOptions.Name, source.Id);

        return source;
    }

    private async Task<Source> GetSource(string name, string location)
    {
        var existing = await _mediator.Send(new GetSourceByNameQuery { Name = name });
        var source = Source.Create(name, location);
        
        if (existing != null) source.Id = existing.Id;
        
        return source;
    }

    private async Task<IEnumerable<Repository>> ParseRepositoriesAsync(SourceOptions sourceOptions)
    {
        var paths = sourceOptions.LocationInfo.GetGitDirectories();

        return await Task.WhenAll(paths
            .Where(p => !sourceOptions.IsExcluded(p))
            .Select(ParseRepositoryAsync));
    }

    private async Task<Repository> ParseRepositoryAsync(string repositoryPath)
    {
        _context.GitDirectory = new DirectoryInfo(repositoryPath);

        _logger.LogInformation("Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        var repository = await _repositoryParser.ParseAsync(_context);

        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", _context.GitRepositoryName);

        return repository;
    }

    public void Dispose() => _context.Dispose();
}