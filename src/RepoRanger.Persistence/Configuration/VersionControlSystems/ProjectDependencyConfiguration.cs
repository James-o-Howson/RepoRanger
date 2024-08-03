using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Persistence.Configuration.VersionControlSystems;

internal sealed class ProjectDependencyConfiguration : IEntityTypeConfiguration<ProjectDependency>
{
    public void Configure(EntityTypeBuilder<ProjectDependency> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.Dependency)
            .WithMany()
            .HasForeignKey(d => d.DependencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Version)
            .WithMany()
            .HasForeignKey(d => d.VersionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(d => d.Source)
            .WithMany()
            .HasForeignKey(d => d.SourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(d => d.Created)
            .IsRequired();
    }
}