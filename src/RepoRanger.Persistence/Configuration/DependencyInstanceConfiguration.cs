using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class DependencyInstanceConfiguration : IEntityTypeConfiguration<DependencyInstance>
{
    public void Configure(EntityTypeBuilder<DependencyInstance> builder)
    {
        builder.HasKey(di => di.Id);
        
        builder.HasAlternateKey(d => new
        {
            d.ProjectId, d.DependencyName
        });
        
        builder.OwnsOne(d => d.Source);
        
        builder.Property(di => di.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(di => di.Created)
            .IsRequired();
    }
}