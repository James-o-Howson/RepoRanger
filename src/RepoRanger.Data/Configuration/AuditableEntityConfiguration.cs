using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Common;

namespace RepoRanger.Data.Configuration;

internal class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
    where TEntity : BaseAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(v => v.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(v => v.Created)
            .IsRequired();
        
        builder.Property(v => v.LastModifiedBy)
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(v => v.LastModified);
    }
}