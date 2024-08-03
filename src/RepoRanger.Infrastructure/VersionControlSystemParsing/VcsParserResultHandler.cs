using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;
using RepoRanger.Domain.VersionControlSystems.Updaters;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing;

internal interface IVcsParserResultHandler : IDisposable
{
    Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken);
}

internal sealed class VcsParserResultHandler : IVcsParserResultHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IVersionControlSystemFactory _factory;
    private readonly IVersionControlSystemUpdater _updater;
    private readonly IDependencyManager _dependencyManager;

    public VcsParserResultHandler(IApplicationDbContext dbContext,
        IVersionControlSystemUpdater updater,
        IVersionControlSystemFactory factory,
        IDependencyManager dependencyManager)
    {
        _dbContext = dbContext;
        _updater = updater;
        _factory = factory;
        _dependencyManager = dependencyManager;
    }

    public async Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(results);
        await InitDependencyManager(cancellationToken);
        
        var versionControlSystems = await GetVersionControlSystemsAsync(cancellationToken);
        var descriptors = results.Select(r => r.VersionControlSystemDescriptor);
        
        await Synchronize(descriptors, versionControlSystems, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task Synchronize(IEnumerable<VersionControlSystemDescriptor> descriptors,
        List<VersionControlSystem> versionControlSystems, CancellationToken cancellationToken)
    {
        List<VersionControlSystem> newVersionControlSystems = [];
        foreach (var descriptor in descriptors)
        {
            var vcs = CreateOrUpdate(descriptor, versionControlSystems);
            newVersionControlSystems.Add(vcs);
        }

        await _dbContext.VersionControlSystems.AddRangeAsync(newVersionControlSystems, cancellationToken);
    }

    private VersionControlSystem CreateOrUpdate(VersionControlSystemDescriptor descriptor, 
        List<VersionControlSystem> versionControlSystems)
    {
        var existing = versionControlSystems.SingleOrDefault(v => v.Name == descriptor.Name);
        if (existing is null)
        {
            var versionControlSystem = _factory.Create(_dependencyManager, descriptor);
            return versionControlSystem;
        }

        _updater.Update(existing, descriptor, _dependencyManager);
        return existing;
    }
    
    private async Task InitDependencyManager(CancellationToken cancellationToken)
    {
        var dependencies = await GetDependenciesAsync(cancellationToken);
        var versions = await GetDependencyVersionsAsync(cancellationToken);
        var sources = await GetDependencySourcesAsync(cancellationToken);

        _dependencyManager.Manage(dependencies, versions, sources);
    }

    private async Task<List<Dependency>> GetDependenciesAsync(CancellationToken cancellationToken) =>
        await _dbContext.Dependencies
            .Include(d => d.Versions)
            .ThenInclude(v => v.Sources)
            .ToListAsync(cancellationToken);
    
    private async Task<List<DependencyVersion>> GetDependencyVersionsAsync(CancellationToken cancellationToken) =>
        await _dbContext.DependencyVersions
            .Include(d => d.Sources)
            .Include(v => v.Dependency)
            .Include(v => v.Sources)
            .ToListAsync(cancellationToken);
    
    private async Task<List<DependencySource>> GetDependencySourcesAsync(CancellationToken cancellationToken) =>
        await _dbContext.DependencySources
            .Include(s => s.Versions)
            .ToListAsync(cancellationToken);

    private async Task<List<VersionControlSystem>> GetVersionControlSystemsAsync(CancellationToken cancellationToken) =>
        await _dbContext.VersionControlSystems
            // Eager Load Dependency on Project Dependency
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .ThenInclude(pd => pd.Dependency)
            .ThenInclude(d => d.Versions)
            .ThenInclude(v => v.Sources)
            .ThenInclude(s => s.Versions)
            
            // Eager Load Version on Project Dependency
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .ThenInclude(pd => pd.Version)
            .ThenInclude(v => v.Dependency)
            
            // Eager Load Source on Project Dependency
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .ThenInclude(pd => pd.Source)
            .ThenInclude(s => s.Versions)
            .ToListAsync(cancellationToken);


    public void Dispose()
    {
        _dependencyManager.Dispose();
    }
}