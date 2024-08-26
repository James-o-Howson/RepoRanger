using Microsoft.EntityFrameworkCore;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.PersistedEvents;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Application.Abstractions.Interfaces.Persistence;

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
    DbSet<PersistedEvent> PersistedEvents { get; set; }
    DbSet<Vulnerability> Vulnerabilities { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}