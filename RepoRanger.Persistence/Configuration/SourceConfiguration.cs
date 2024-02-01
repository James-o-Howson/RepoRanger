using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class SourceConfiguration : GuidBaseAuditableEntityConfiguration<Source>
{
    public override void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.HasMany(r => r.Repositories)
            .WithOne()
            .HasForeignKey(b => b.SourceId)
            .IsRequired();
        
        base.Configure(builder);
    }
}