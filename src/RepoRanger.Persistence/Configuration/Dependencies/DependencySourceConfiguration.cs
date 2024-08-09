using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Persistence.Configuration.Dependencies;

internal sealed class DependencySourceConfiguration : IEntityTypeConfiguration<DependencySource>
{
    public void Configure(EntityTypeBuilder<DependencySource> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new DependencySourceId(value))
            .ValueGeneratedNever();
        
        builder.HasIndex(s => s.Name).IsUnique();
    }
}