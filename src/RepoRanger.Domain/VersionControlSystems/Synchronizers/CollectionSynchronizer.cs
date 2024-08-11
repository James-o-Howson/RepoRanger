using RepoRanger.Domain.Common;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

internal sealed class CollectionSynchronizer<TEntity, TChangeDescriptor>
    where TEntity : BaseEntity, IAlternateKeyProvider
    where TChangeDescriptor : IAlternateKeyProvider
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

        var persistedMap = persistedCollection.ToDictionary(p => p.GetAlternateKey);
        var descriptorMap = descriptors.ToDictionary(c => c.GetAlternateKey);
        
        foreach (var descriptor in descriptors)
        {
            var descriptorKey = descriptor.GetAlternateKey;
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

    private void HandleDelete(Dictionary<AlternateKey, TEntity> persistedMap, Dictionary<AlternateKey, TChangeDescriptor> descriptorMap)
    {
        var entitiesToDelete = persistedMap.Values
            .Where(persisted => !descriptorMap.ContainsKey(persisted.GetAlternateKey))
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