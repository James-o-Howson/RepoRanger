using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Messages;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<VersionControlSystem> VersionControlSystems { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMetadata> ProjectMetadata { get; set; }
    public DbSet<ProjectDependency> ProjectDependencies { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<DependencySource> DependencySources { get; set; }
    public DbSet<DependencyVersion> DependencyVersions { get; set; }

    public DbSet<Message> Messages { get; set; }
    // public DbSet<Vulnerability> Vulnerabilities { get; set; }

    public void MarkModified<TEntity>(TEntity entity) where TEntity : class
    {
        Entry(entity).State = EntityState.Modified;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}