using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Abstractions.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<SourceDetail> SourceDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}