using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Common;

namespace RepoRanger.Persistence.Configuration;

internal abstract class GuidBaseCreatedAuditableEntityConfiguration<TEntity> : BaseCreatedAuditableEntityConfiguration<TEntity, Guid>
    where TEntity : BaseCreatedAuditableEntity<Guid>
{
}

internal abstract class BaseCreatedAuditableEntityConfiguration<TEntity, TId> : BaseEntityConfiguration<TEntity, TId>
    where TEntity : BaseCreatedAuditableEntity<TId>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(e => e.Created)
            .IsRequired();
            
        base.Configure(builder);
    }
}