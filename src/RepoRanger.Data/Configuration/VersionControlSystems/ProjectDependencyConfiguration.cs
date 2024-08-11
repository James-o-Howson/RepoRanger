using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Data.Configuration.VersionControlSystems;

internal sealed class ProjectDependencyConfiguration : AuditableEntityConfiguration<ProjectDependency>
{
    public override void Configure(EntityTypeBuilder<ProjectDependency> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => new ProjectDependencyId(value))
            .ValueGeneratedNever();

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

        base.Configure(builder);
    }
}