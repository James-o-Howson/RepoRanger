using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies;

namespace RepoRanger.Persistence.Configuration;

internal sealed class DependencyConfiguration : IEntityTypeConfiguration<Dependency>
{
    public void Configure(EntityTypeBuilder<Dependency> builder)
    {
        builder.HasKey(d => d.Name);
        
        // builder.HasMany(d => d.DependencyInstances)
        //     .WithOne()
        //     .HasForeignKey(di => di.DependencyName);
    }
}