using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RepoRanger.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private static readonly DbContextOptions DefaultOptions = new DbContextOptionsBuilder().UseSqlServer().Options;
    
    public ApplicationDbContext() : base(DefaultOptions) {}
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}