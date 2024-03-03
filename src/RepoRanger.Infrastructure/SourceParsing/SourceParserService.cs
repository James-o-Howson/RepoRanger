using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class SourceParserService : ISourceParserService, IDisposable
{
    private readonly ParsingContext _parseContext;

    private readonly ILogger<SourceParserService> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly SourceParserOptions _options;
    private readonly IRepositoryParser _repositoryParser;

    public SourceParserService(IEnumerable<ISourceFileParser> fileContentParsers,
        IOptions<SourceParserOptions> options,
        ILogger<SourceParserService> logger, 
        IRepositoryParser repositoryParser, 
        IApplicationDbContext dbContext)
    {
        ConcurrentQueue<ISourceFileParser> sourceFileParsers = new(fileContentParsers);
        _logger = logger;
        _repositoryParser = repositoryParser;
        _dbContext = dbContext;
        _options = options.Value;

        _parseContext = ParsingContext.Create(sourceFileParsers);
    }

    private IEnumerable<SourceOptions> EnabledSourceOptions => 
        _options.Sources.Where(s => s.Enabled);

    public async Task ParseAsync(CancellationToken cancellationToken)
    {
        var sources = await Task.WhenAll(EnabledSourceOptions.Select(options => 
            ParseSourceAsync(options, cancellationToken)));

        foreach (var source in sources)
        {
            await CreateOrUpdate(source, cancellationToken);
        }
    }

    private async Task CreateOrUpdate(Source source, CancellationToken cancellationToken)
    {
        if (source.IsNew)
        {
            await _dbContext.Sources.AddAsync(source, cancellationToken);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

    }

    private async Task<Source> ParseSourceAsync(SourceOptions sourceOptions, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);

        var source = await GetSource(sourceOptions.Name, sourceOptions.Location, cancellationToken);
        _parseContext.Source = source;
            
        var repositories = await ParseRepositoriesAsync(sourceOptions);
        source.AddRepositories(repositories);

        _logger.LogInformation("Finished Parsing Source {SourceName}", sourceOptions.Name);

        return source;
    }

    private async Task<Source> GetSource(string name, string location, CancellationToken cancellationToken)
    {
        var source = await _dbContext.Sources
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

        return source ?? Source.Create(name, location);
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
        _parseContext.GitDirectory = new DirectoryInfo(repositoryPath);

        _logger.LogInformation("Parsing Repository {RepositoryName}", _parseContext.GitRepositoryName);

        var repository = await _repositoryParser.ParseAsync(_parseContext);

        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", _parseContext.GitRepositoryName);

        return repository;
    }

    public void Dispose() => _parseContext.Dispose();
}