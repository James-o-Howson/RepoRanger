using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;
using RepoRanger.Domain.VersionControlSystems.Updaters;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing;

internal interface IVcsParserResultHandler
{
    Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken);
}

internal sealed class VcsParserResultHandler : IVcsParserResultHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IVersionControlSystemFactory _factory;
    private readonly IVersionControlSystemUpdater _updater;
    private readonly IDependencyManagerFactory _dependencyManagerFactory;

    public VcsParserResultHandler(IApplicationDbContext dbContext,
        IVersionControlSystemUpdater updater,
        IVersionControlSystemFactory factory,
        IDependencyManagerFactory dependencyManagerFactory)
    {
        _dbContext = dbContext;
        _updater = updater;
        _factory = factory;
        _dependencyManagerFactory = dependencyManagerFactory;
    }

    public async Task HandleAsync(IEnumerable<VersionControlSystemParserResult> results, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(results);
        
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
            var vcs = await CreateOrUpdate(descriptor, versionControlSystems, cancellationToken);
            newVersionControlSystems.Add(vcs);
        }

        await _dbContext.VersionControlSystems.AddRangeAsync(newVersionControlSystems, cancellationToken);
    }

    private async Task<VersionControlSystem> CreateOrUpdate(VersionControlSystemDescriptor descriptor, 
        List<VersionControlSystem> versionControlSystems, CancellationToken cancellationToken)
    {
        var dependencyManager = await _dependencyManagerFactory.CreateAsync(cancellationToken);
        var existing = versionControlSystems.SingleOrDefault(v => v.Name == descriptor.Name);
        if (existing is null)
        {
            var versionControlSystem = _factory.Create(dependencyManager, descriptor);
            return versionControlSystem;
        }

        _updater.Update(existing, descriptor, dependencyManager);
        return existing;
    }

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
}