// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public static class EntityEntryExtensions
{
    public static bool IsCreated(this EntityEntry entry) => 
        entry.State == EntityState.Added;

    public static bool IsModified(this EntityEntry entry) =>
        entry.State == EntityState.Added || 
        entry.State == EntityState.Modified ||
        entry.HasChangedOwnedEntities();

    private static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}