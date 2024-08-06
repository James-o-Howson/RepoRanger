using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Infrastructure.Services;

internal sealed class DependencyManagerFactory : IDependencyManagerFactory
{
    private readonly IDependencyManager _dependencyManager;
    private readonly IApplicationDbContext _dbContext;

    public DependencyManagerFactory(IDependencyManager dependencyManager, IApplicationDbContext dbContext)
    {
        _dependencyManager = dependencyManager;
        _dbContext = dbContext;
    }

    public async Task<IDependencyManager> CreateAsync(CancellationToken cancellationToken = default)
    {
        var dependencies = await GetDependenciesAsync(cancellationToken);
        var versions = await GetDependencyVersionsAsync(cancellationToken);
        var sources = await GetDependencySourcesAsync(cancellationToken);

        _dependencyManager.Manage(dependencies, versions, sources);

        return _dependencyManager;
    }
    
    private async Task<List<Dependency>> GetDependenciesAsync(CancellationToken cancellationToken) =>
        await _dbContext.Dependencies
            .Include(d => d.Versions)
            .ThenInclude(v => v.Sources)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    
    private async Task<List<DependencyVersion>> GetDependencyVersionsAsync(CancellationToken cancellationToken) =>
        await _dbContext.DependencyVersions
            .Include(d => d.Sources)
            .Include(v => v.Dependency)
            .Include(v => v.Sources)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    
    private async Task<List<DependencySource>> GetDependencySourcesAsync(CancellationToken cancellationToken) =>
        await _dbContext.DependencySources
            .Include(s => s.Versions)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
}