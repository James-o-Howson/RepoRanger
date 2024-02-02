using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Source;

namespace RepoRanger.Persistence.Configuration;

internal sealed class RepositoryConfiguration : GuidBaseAuditableEntityConfiguration<Repository>
{
    public override void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasMany(r => r.Branches)
            .WithOne()
            .HasForeignKey(b => b.RepositoryId)
            .IsRequired();
        
        base.Configure(builder);
    }
}