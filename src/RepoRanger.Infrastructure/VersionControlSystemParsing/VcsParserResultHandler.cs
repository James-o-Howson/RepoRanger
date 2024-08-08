using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;
using RepoRanger.Domain.VersionControlSystems.Updaters;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing;

internal interface IVcsParserResultHandler
{
    Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken);
}

internal sealed class VcsParserResultHandler : IVcsParserResultHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDependencyManagerFactory _dependencyManagerFactory;
    private readonly IVersionControlSystemFactory _factory;
    private readonly IVersionControlSystemUpdater _updater;

    public VcsParserResultHandler(IApplicationDbContext dbContext,
        IDependencyManagerFactory dependencyManagerFactory, 
        IVersionControlSystemFactory factory,
        IVersionControlSystemUpdater updater)
    {
        _dbContext = dbContext;
        _dependencyManagerFactory = dependencyManagerFactory;
        _factory = factory;
        _updater = updater;
    }

    public async Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(results);
        
        var existing = await GetVersionControlSystemsAsync(cancellationToken);
        var descriptors = results.Select(r => r.VersionControlSystemDescriptor);
        var dependencyManager = await _dependencyManagerFactory.CreateAsync(cancellationToken);
        
        foreach (var descriptor in descriptors)
        {
            await Synchronize(existing, descriptor, dependencyManager, cancellationToken);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task Synchronize(List<VersionControlSystem> existing, VersionControlSystemDescriptor descriptor,
        IDependencyManager dependencyManager, CancellationToken cancellationToken)
    {
        var match = existing.SingleOrDefault(v => v.Name == descriptor.Name);
        if (match is null)
        {
            var versionControlSystem = _factory.Create(dependencyManager, descriptor);
            await _dbContext.VersionControlSystems.AddAsync(versionControlSystem, cancellationToken);
            return;
        }

        _updater.Update(match, descriptor, dependencyManager);
        _dbContext.MarkModified(match);
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
}