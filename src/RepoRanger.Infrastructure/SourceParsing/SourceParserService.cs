using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Infrastructure.SourceParsing.Common;

namespace RepoRanger.Infrastructure.SourceParsing;

internal sealed class SourceParserService : ISourceParserService
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
        var parseResults = await Task.WhenAll(
            EnabledSourceOptions.Select(options => ParseSourceAsync(options, cancellationToken)));

        foreach (var result in parseResults)
        {
            await CreateOrUpdate(result, cancellationToken);
        }
    }

    private async Task CreateOrUpdate(ParsedSourceResult result, CancellationToken cancellationToken)
    {
        if (result.IsNewSource)
        {
            await CreateOrUpdateDependencies(result.Parsed.Dependencies, cancellationToken);
            await _dbContext.Sources.AddAsync(result.Parsed, cancellationToken);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(result.Existing);
            await CreateOrUpdateDependencies(result.Existing.Dependencies, cancellationToken);
            result.Existing.Update(result.Existing.Location, result.Existing.Repositories.ToList());
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateOrUpdateDependencies(IEnumerable<string> dependencies, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Dependencies.ToListAsync(cancellationToken);
        await _dbContext.Dependencies.AddRangeAsync(dependencies.Select(Dependency.Create).Except(existing), cancellationToken);
    }

    private async Task<ParsedSourceResult> ParseSourceAsync(SourceOptions sourceOptions, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parsing Source {SourceName}", sourceOptions.Name);
        
        var repositories = await ParseRepositoriesAsync(sourceOptions);
        var source = Source.Create(sourceOptions.Name, sourceOptions.Location, repositories);
        
        var existing = await _dbContext.Sources
            .FirstOrDefaultAsync(s => s.Name == sourceOptions.Name, cancellationToken);

        _logger.LogInformation("Finished Parsing Source {SourceName}", sourceOptions.Name);
        
        return ParsedSourceResult.CreateInstance(existing, source);
    }
    
    private async Task<IEnumerable<Repository>> ParseRepositoriesAsync(SourceOptions sourceOptions)
    {
        var gitRepositories = sourceOptions.LocationInfo.GetGitRepositories();

        return await Task.WhenAll(gitRepositories
            .Where(p => !sourceOptions.IsExcluded(p))
            .Select(ParseRepositoryAsync));
    }

    private async Task<Repository> ParseRepositoryAsync(string repositoryPath)
    {
        var gitRepository = new DirectoryInfo(repositoryPath);

        _logger.LogInformation("Parsing Repository {RepositoryName}", gitRepository.FullName);

        var repository = await _repositoryParser.ParseAsync(gitRepository, _parseContext);

        _logger.LogInformation("Finished Parsing Repository {RepositoryName}", gitRepository.Name);

        return repository;
    }
}