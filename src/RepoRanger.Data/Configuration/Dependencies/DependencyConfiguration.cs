using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Data.Configuration.Dependencies;

internal sealed class DependencyConfiguration : AuditableEntityConfiguration<Dependency>
{
    public override void Configure(EntityTypeBuilder<Dependency> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new DependencyId(value))
            .ValueGeneratedNever();
        
        builder.HasIndex(d => d.Name).IsUnique();

        builder.HasMany(d => d.Versions)
            .WithOne(v => v.Dependency)
            .HasForeignKey(v => v.DependencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}