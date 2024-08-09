using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class ProjectMetadataConfiguration : IEntityTypeConfiguration<ProjectMetadata>
{
    public void Configure(EntityTypeBuilder<ProjectMetadata> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new ProjectMetadataId(value))
            .ValueGeneratedNever();
    }
}