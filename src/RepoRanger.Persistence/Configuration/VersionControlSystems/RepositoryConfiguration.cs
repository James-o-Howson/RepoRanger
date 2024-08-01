using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasAlternateKey(r => new
        {
            SourceId = r.VersionControlSystemId, r.Name, r.RemoteUrl
        });

        builder.HasMany(r => r.Projects)
            .WithOne(p => p.Repository)
            .HasForeignKey(b => b.RepositoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();
    }
}