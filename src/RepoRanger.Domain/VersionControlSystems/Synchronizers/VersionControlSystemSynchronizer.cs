using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Updaters;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

public interface IVersionControlSystemSynchronizer
{
    Task<SynchronizationResult> SynchronizeAsync(IEnumerable<VersionControlSystem> versionControlSystems,
        IEnumerable<VersionControlSystemDescriptor> descriptors,
        CancellationToken cancellationToken);
}

internal sealed class VersionControlSystemSynchronizer : IVersionControlSystemSynchronizer
{
    private readonly IDependencyManagerFactory _dependencyManagerFactory;
    private readonly IVersionControlSystemFactory _factory;
    private readonly IVersionControlSystemUpdater _updater;
    
    private readonly List<VersionControlSystem> _toAdd = [];
    private readonly List<VersionControlSystem> _toDelete = [];
    private readonly List<VersionControlSystem> _updated = [];

    public VersionControlSystemSynchronizer(IDependencyManagerFactory dependencyManagerFactory,IVersionControlSystemFactory factory, IVersionControlSystemUpdater updater)
    {
        _dependencyManagerFactory = dependencyManagerFactory;
        _factory = factory;
        _updater = updater;
    }

    public async Task<SynchronizationResult> SynchronizeAsync(IEnumerable<VersionControlSystem> versionControlSystems,
        IEnumerable<VersionControlSystemDescriptor> descriptors,
        CancellationToken cancellationToken)
    {
        DomainException.ThrowIfNull(versionControlSystems);
        DomainException.ThrowIfNull(descriptors);
        
        var dependencyManager = await _dependencyManagerFactory.CreateAsync(cancellationToken);
        
        var synchronizer = GetSynchronizer(dependencyManager);
        synchronizer.Synchronize(versionControlSystems, descriptors);

        return new SynchronizationResult
        {
            ToAdd = _toAdd,
            ToRemove = _toDelete,
            Updated = _updated
        };
    }

    private CollectionSynchronizer<VersionControlSystem, VersionControlSystemDescriptor> GetSynchronizer(IDependencyManager dependencyManager)
    {
        return new CollectionSynchronizer<VersionControlSystem, VersionControlSystemDescriptor>(OnNew, OnUpdate, OnDelete);

        void OnNew(VersionControlSystemDescriptor descriptor) => 
            _toAdd.Add(_factory.Create(dependencyManager, descriptor));

        void OnUpdate(VersionControlSystem versionControlSystem, VersionControlSystemDescriptor descriptor)
        {
            _updater.Update(versionControlSystem, descriptor, dependencyManager);
            _updated.Add(versionControlSystem);
        }

        void OnDelete(VersionControlSystem versionControlSystem) => 
            _toDelete.Add(versionControlSystem);
    }
}