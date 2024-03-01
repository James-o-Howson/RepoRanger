using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.Sources;

namespace RepoRanger.Persistence.Configuration;

internal sealed class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.HasKey(e => e.Id);
            
        builder.HasMany(s => s.Repositories)
            .WithOne(s => s.Source)
            .HasForeignKey(s => s.SourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.Name)
            .IsUnique();
        
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();
    }
}