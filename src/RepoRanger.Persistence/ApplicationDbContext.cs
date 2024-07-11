using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Source> Sources { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Metadata> Metadata { get; set; }
    public DbSet<DependencyInstance> DependencyInstances { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}