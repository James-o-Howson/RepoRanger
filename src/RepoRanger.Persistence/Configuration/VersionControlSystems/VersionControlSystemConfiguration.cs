using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class VersionControlSystemConfiguration : AuditableEntityConfiguration<VersionControlSystem>
{
    public override void Configure(EntityTypeBuilder<VersionControlSystem> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => new VersionControlSystemId(value))
            .ValueGeneratedNever();
            
        builder.HasMany(v => v.Repositories)
            .WithOne(r => r.VersionControlSystem)
            .HasForeignKey(r => r.VersionControlSystemId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.Name)
            .IsUnique();
        
        base.Configure(builder);
    }
}