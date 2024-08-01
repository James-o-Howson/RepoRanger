using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class VersionControlSystemConfiguration : IEntityTypeConfiguration<VersionControlSystem>
{
    public void Configure(EntityTypeBuilder<VersionControlSystem> builder)
    {
        builder.HasKey(e => e.Id);
            
        builder.HasMany(s => s.Repositories)
            .WithOne(s => s.VersionControlSystem)
            .HasForeignKey(s => s.VersionControlSystemId)
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