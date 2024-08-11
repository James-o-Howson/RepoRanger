using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;
using RepoRanger.Domain.VersionControlSystems.Synchronizers;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing;

internal sealed class VcsParserService : IVersionControlSystemParserService
{
    private readonly IVersionControlSystemParser _versionControlSystemParser;
    private readonly VersionControlSystemContexts _options;
    private readonly IApplicationDbContext _dbContext;
    private readonly IVersionControlSystemSynchronizer _synchronizer;
    private readonly ILogger<VcsParserService> _logger;

    public VcsParserService(
        IOptions<VersionControlSystemContexts> options,
        IVersionControlSystemParser versionControlSystemParser, 
        IApplicationDbContext dbContext, 
        IVersionControlSystemSynchronizer synchronizer, 
        ILogger<VcsParserService> logger)
    {
        _versionControlSystemParser = versionControlSystemParser;
        _dbContext = dbContext;
        _synchronizer = synchronizer;
        _logger = logger;
        _options = options.Value;
    }

    private IEnumerable<VersionControlSystemContext> EnabledVcsOptions => 
        _options.Values.Where(s => s.Enabled);

    public async Task ParseAsync(CancellationToken cancellationToken)
    {
        var descriptors = await _versionControlSystemParser.ParseAsync(EnabledVcsOptions, cancellationToken);

        await HandleResults(descriptors, cancellationToken);
    }
    
    private async Task HandleResults(IEnumerable<VersionControlSystemDescriptor> descriptors, CancellationToken cancellationToken)
    {
        var results = await SynchronizeAsync(descriptors, cancellationToken);

        await _dbContext.VersionControlSystems.AddRangeAsync(results.ToAdd, cancellationToken);
        _dbContext.VersionControlSystems.RemoveRange(results.ToRemove);
      
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        LogResults(results);
    }

    private async Task<SynchronizationResult> SynchronizeAsync(IEnumerable<VersionControlSystemDescriptor> descriptors, CancellationToken cancellationToken)
    {
        var existing = await GetVersionControlSystemsAsync(cancellationToken);
        return await _synchronizer.SynchronizeAsync(existing, descriptors, cancellationToken);
    }

    private async Task<List<VersionControlSystem>> GetVersionControlSystemsAsync(CancellationToken cancellationToken) =>
        await _dbContext.VersionControlSystems
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Projects)
            .ThenInclude(p => p.Metadata)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    private void LogResults(SynchronizationResult results)
    {
        _logger.LogInformation("Repositories Added: {RepositoriesAdded}", results.ToAdd.GetAllRepositoryNames());
        _logger.LogInformation("Repositories Updated: {RepositoriesUpdated}", results.Updated.GetAllRepositoryNames());
        _logger.LogInformation("Repositories Removed: {RepositoriesRemoved}", results.ToRemove.GetAllRepositoryNames());
    }
}