using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers.Updaters;

internal interface IVersionControlSystemUpdater
{
    void Update(VersionControlSystem versionControlSystem, VersionControlSystemDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class VersionControlSystemUpdater : IVersionControlSystemUpdater
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepositoryUpdater _repositoryUpdater;

    public VersionControlSystemUpdater(IRepositoryFactory repositoryFactory,
        IRepositoryUpdater repositoryUpdater)
    {
        _repositoryFactory = repositoryFactory;
        _repositoryUpdater = repositoryUpdater;
    }
    
    public void Update(VersionControlSystem versionControlSystem, VersionControlSystemDescriptor descriptor, IDependencyManager dependencyManager)
    {
        versionControlSystem.Location = descriptor.Location;
        SynchronizeRepositories(versionControlSystem, descriptor, dependencyManager);
    }

    private void SynchronizeRepositories(VersionControlSystem versionControlSystem,
        VersionControlSystemDescriptor changeDescriptor, IDependencyManager dependencyManager)
    {
        var synchronizer = new CollectionSynchronizer<Repository, RepositoryDescriptor>(OnNew, OnUpdate, OnDelete);
        synchronizer.Synchronize(versionControlSystem.Repositories, changeDescriptor.Repositories);
        return;

        void OnNew(RepositoryDescriptor descriptor) => 
            versionControlSystem.AddRepository(
                _repositoryFactory.Create(versionControlSystem, descriptor, dependencyManager));

        void OnUpdate(Repository repository, RepositoryDescriptor descriptor) => 
            _repositoryUpdater.Update(repository, descriptor, dependencyManager);

        void OnDelete(Repository repository) => 
            versionControlSystem.DeleteRepository(repository.Id);
    }
}