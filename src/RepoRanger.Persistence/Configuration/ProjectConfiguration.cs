using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.HasAlternateKey(p => new
        {
            p.RepositoryId, p.Name, p.Path
        });
        
        builder.HasMany(p => p.DependencyInstances)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId);

        builder.OwnsOne(p => p.Type);
        builder.OwnsOne(p => p.Metadata);
        
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();
    }
}