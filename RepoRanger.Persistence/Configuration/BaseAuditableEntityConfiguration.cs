using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Common;

namespace RepoRanger.Persistence.Configuration;

internal abstract class GuidBaseAuditableEntityConfiguration<TEntity> : BaseAuditableEntityConfiguration<TEntity, Guid>
    where TEntity : BaseAuditableEntity<Guid>
{
}

internal abstract class BaseAuditableEntityConfiguration<TEntity, TId> : BaseEntityConfiguration<TEntity, TId>
    where TEntity : BaseAuditableEntity<TId>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();

        builder.Property(e => e.LastModifiedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.LastModified)
            .IsRequired();
            
        base.Configure(builder);
    }
}