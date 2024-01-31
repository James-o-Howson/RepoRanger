using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class ProjectConfiguration : GuidBaseAuditableEntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(b => b.Dependencies)
            .WithMany(p => p.Projects);

        builder.OwnsOne(p => p.Type);
        
        base.Configure(builder);
    }
}