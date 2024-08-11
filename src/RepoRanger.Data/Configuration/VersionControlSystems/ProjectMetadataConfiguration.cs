using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Data.Configuration.VersionControlSystems;

internal sealed class ProjectMetadataConfiguration : AuditableEntityConfiguration<ProjectMetadata>
{
    public override void Configure(EntityTypeBuilder<ProjectMetadata> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new ProjectMetadataId(value))
            .ValueGeneratedNever();
        
        base.Configure(builder);
    }
}