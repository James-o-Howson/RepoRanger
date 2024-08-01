using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies;

namespace RepoRanger.Persistence.Configuration.Dependencies;

internal sealed class DependencyConfiguration : IEntityTypeConfiguration<Dependency>
{
    public void Configure(EntityTypeBuilder<Dependency> builder)
    {
        builder.HasKey(d => d.Id);
        
        builder.HasIndex(d => d.Name).IsUnique();

        builder.HasMany(d => d.Versions)
            .WithOne(v => v.Dependency)
            .HasForeignKey(v => v.DependencyId);
    }
}