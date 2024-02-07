using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Source;

namespace RepoRanger.Persistence.Configuration;

internal sealed class SourceConfiguration : GuidBaseCreatedAuditableEntityConfiguration<Source>
{
    public override void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.HasMany(r => r.Repositories)
            .WithOne(r => r.Source)
            .HasForeignKey(r => r.SourceId)
            .IsRequired();
        
        base.Configure(builder);
    }
}