using Microsoft.EntityFrameworkCore;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Abstractions.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Repository> Repositories { get; set; }
    DbSet<Branch> Branches { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<Dependency> Dependencies { get; set; }
    DbSet<Source> Sources { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}