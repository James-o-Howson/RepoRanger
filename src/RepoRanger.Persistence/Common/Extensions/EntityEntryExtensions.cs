using RepoRanger.Domain.Common;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public static class EntityEntryExtensions
{
    public static List<TEntity> WithEvents<TEntity>(
        this IEnumerable<EntityEntry<TEntity>> entityEntries) where TEntity : Entity =>
        entityEntries.Where(e => e.Entity.GetEvents().Count != 0)
            .Select(e => e.Entity)
            .ToList();

    public static bool CanSetCreatedAuditData(this EntityEntry entry) => 
        entry.State == EntityState.Added;

    public static bool CanSetModifiedAuditData(this EntityEntry entry) =>
        entry.State == EntityState.Added || 
        entry.State == EntityState.Modified ||
        entry.HasChangedOwnedEntities();

    private static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}