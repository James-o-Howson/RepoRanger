using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Common;

namespace RepoRanger.Persistence.Configuration;

internal abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> 
    where TEntity : BaseEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
    }
}