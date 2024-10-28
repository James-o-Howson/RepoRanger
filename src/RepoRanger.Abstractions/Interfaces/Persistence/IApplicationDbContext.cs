using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Abstractions.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<VersionControlSystem> VersionControlSystems { get; set; }
    DbSet<Repository> Repositories { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<ProjectDependency> ProjectDependencies { get; set; }
    DbSet<Dependency> Dependencies { get; set; }
    DbSet<DependencyVersion> DependencyVersions { get; set; }
    DbSet<DependencySource> DependencySources { get; set; }
    DbSet<ProjectMetadata> ProjectMetadata { get; set; }
    DbSet<OutboxMessage> OutboxMessages { get; set; }
    DbSet<Vulnerability> Vulnerabilities { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;
}