using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Persistence.Configuration.Dependencies;

internal sealed class DependencyVersionConfiguration : AuditableEntityConfiguration<DependencyVersion>
{
    public override void Configure(EntityTypeBuilder<DependencyVersion> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new DependencyVersionId(value))
            .ValueGeneratedNever();
        
        builder.Property(v => v.Value);
        
        builder.HasMany(v => v.Sources)
            .WithMany(s => s.Versions);

        builder.HasMany(v => v.Vulnerabilities)
            .WithOne(v => v.DependencyVersion)
            .HasForeignKey(v => v.DependencyVersionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}