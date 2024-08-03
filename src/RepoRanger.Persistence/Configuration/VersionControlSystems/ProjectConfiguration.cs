using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.HasAlternateKey(p => new
        {
            p.RepositoryId, p.Name, p.Path
        });
        
        builder.HasMany(p => p.ProjectDependencies)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Metadata)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(p => p.Type);
        
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();
    }
}