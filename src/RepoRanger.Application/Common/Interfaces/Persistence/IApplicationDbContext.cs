using Microsoft.EntityFrameworkCore;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.Sources;
using RepoRanger.Domain.Sources.Repositories;

namespace RepoRanger.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Source> Sources { get; set; }
    DbSet<Repository> Repositories { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<DependencyInstance> DependencyInstances { get; set; }
    DbSet<Dependency> Dependencies { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}