using Microsoft.EntityFrameworkCore;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.Vulnerabilities;

namespace RepoRanger.Application.Abstractions.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Source> Sources { get; set; }
    DbSet<Repository> Repositories { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<DependencyInstance> DependencyInstances { get; set; }
    DbSet<Dependency> Dependencies { get; set; }
    DbSet<Metadata> Metadata { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<Vulnerability> Vulnerabilities { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}