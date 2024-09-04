using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Data.Configuration.Dependencies;

internal sealed class DependencySourceConfiguration : AuditableEntityConfiguration<DependencySource>
{
    public override void Configure(EntityTypeBuilder<DependencySource> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new DependencySourceId(value))
            .ValueGeneratedNever();

        builder.OwnsOne(s => s.Name);
        
        base.Configure(builder);
    }
}