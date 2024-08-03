using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Persistence.Configuration.Dependencies;

internal sealed class DependencyVersionConfiguration : IEntityTypeConfiguration<DependencyVersion>
{
    public void Configure(EntityTypeBuilder<DependencyVersion> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Value);
        
        builder.HasMany(v => v.Sources)
            .WithMany(s => s.Versions);

        builder.HasMany(v => v.Vulnerabilities)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}