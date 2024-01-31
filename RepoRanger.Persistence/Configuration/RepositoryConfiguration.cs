using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class RepositoryConfiguration : GuidBaseAuditableEntityConfiguration<Repository>
{
    public override void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.OwnsOne(r => r.Source);
        
        builder.HasMany(r => r.Branches)
            .WithOne()
            .HasForeignKey(b => b.RepositoryId)
            .IsRequired();
        
        base.Configure(builder);
    }
}