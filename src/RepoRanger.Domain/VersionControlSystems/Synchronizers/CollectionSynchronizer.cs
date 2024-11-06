using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;
using RepoRanger.SharedKernel.Base;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

internal sealed class CollectionSynchronizer<TEntity, TChangeDescriptor> : IDisposable
    where TEntity : BaseEntity, IAlternateIdProvider
    where TChangeDescriptor : IAlternateIdProvider 
{
    private Action<TChangeDescriptor>? _onNew;
    private Action<TEntity, TChangeDescriptor>?  _onUpdate;
    private Action<TEntity>?  _onDelete;

    public CollectionSynchronizer(Action<TChangeDescriptor> onNew,
        Action<TEntity, TChangeDescriptor> onUpdate,
        Action<TEntity> onDelete)
    {
        _onNew = onNew;
        _onUpdate = onUpdate;
        _onDelete = onDelete;
    }

    public void Synchronize(IEnumerable<TEntity> persistedCollection, IEnumerable<TChangeDescriptor> descriptorCollection)
    {
        var descriptors = descriptorCollection.ToList();

        var persistedMap = persistedCollection.ToDictionary(p => p.GetAlternateId);
        var descriptorMap = descriptors.ToDictionary(c => c.GetAlternateId);
        
        foreach (var descriptor in descriptors)
        {
            var descriptorKey = descriptor.GetAlternateId;
            if (persistedMap.TryGetValue(descriptorKey, out var persisted))
            {
                _onUpdate?.Invoke(persisted, descriptor);
            }
            else
            {
                _onNew?.Invoke(descriptor);
            }
        }
        
        HandleDelete(persistedMap, descriptorMap);
    }

    private void HandleDelete(Dictionary<AlternateId, TEntity> persistedMap, Dictionary<AlternateId, TChangeDescriptor> descriptorMap)
    {
        var entitiesToDelete = persistedMap.Values
            .Where(persisted => !descriptorMap.ContainsKey(persisted.GetAlternateId))
            .ToList();

        foreach (var toDelete in entitiesToDelete)      
        {
            _onDelete?.Invoke(toDelete);
        }
    }

    public void Dispose()
    {
        _onNew = null;
        _onUpdate = null;
        _onDelete = null;
    }
}