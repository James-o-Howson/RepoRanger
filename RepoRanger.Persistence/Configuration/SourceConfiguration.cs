using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Source;

namespace RepoRanger.Persistence.Configuration;

internal sealed class SourceConfiguration : GuidBaseCreatedAuditableEntityConfiguration<Source>
{
    public override void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.HasMany(s => s.Repositories)
            .WithOne(s => s.Source)
            .HasForeignKey(s => s.SourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.Name)
            .IsUnique();
        
        base.Configure(builder);
    }
}