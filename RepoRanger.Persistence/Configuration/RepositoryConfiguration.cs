using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class RepositoryConfiguration : GuidBaseCreatedAuditableEntityConfiguration<Repository>
{
    public override void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasMany(r => r.Projects)
            .WithOne(p => p.Repository)
            .HasForeignKey(b => b.RepositoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.DefaultBranch)
            .WithOne(b => b.Repository)
            .HasForeignKey<Branch>(b => b.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}