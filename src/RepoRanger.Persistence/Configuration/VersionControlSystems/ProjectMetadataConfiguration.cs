using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class ProjectMetadataConfiguration : IEntityTypeConfiguration<ProjectMetadata>
{
    public void Configure(EntityTypeBuilder<ProjectMetadata> builder)
    {
        builder.HasKey(e => e.Id);
    }
}