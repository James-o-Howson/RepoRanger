using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasKey(e => e.Id);

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