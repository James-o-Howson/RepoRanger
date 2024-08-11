using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Data.Configuration.VersionControlSystems;

internal sealed class RepositoryConfiguration : AuditableEntityConfiguration<Repository>
{
    public override void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new RepositoryId(value))
            .ValueGeneratedNever();

        builder.HasAlternateKey(r => new
        {
            SourceId = r.VersionControlSystemId, r.Name, r.RemoteUrl
        });

        builder.HasMany(r => r.Projects)
            .WithOne(p => p.Repository)
            .HasForeignKey(b => b.RepositoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        base.Configure(builder);
    }
}