using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Source;

namespace RepoRanger.Persistence.Configuration;

internal sealed class BranchConfiguration : GuidBaseCreatedAuditableEntityConfiguration<Branch>
{
    public override void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasMany(b => b.Projects)
            .WithMany(p => p.Branches);
        
        base.Configure(builder);
    }
}