using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class ProjectConfiguration : GuidBaseCreatedAuditableEntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(b => b.Dependencies)
            .WithMany(p => p.Projects);

        base.Configure(builder);
    }
}