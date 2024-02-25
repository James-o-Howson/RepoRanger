using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static async Task<EntityEntry<TEntity>?> AddIfNotExistsAsync<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, 
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) 
        where TEntity : class
    {
        var exists = predicate != null ? 
            await dbSet.AnyAsync(predicate, cancellationToken) : 
            await dbSet.AnyAsync(cancellationToken);
        
        return !exists ? await dbSet.AddAsync(entity, cancellationToken) : null;
    }
}