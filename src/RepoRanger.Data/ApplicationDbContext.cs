using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.Entities;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Data;

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
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<DeadLetterEntry> DeadLetterEntries { get; set; }
    public DbSet<ProcessingFailure> ProcessingFailures { get; set; }
    public DbSet<Vulnerability> Vulnerabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}