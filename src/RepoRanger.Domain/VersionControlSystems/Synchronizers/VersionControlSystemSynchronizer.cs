using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

public interface IVersionControlSystemSynchronizer
{
    void Update(VersionControlSystem target, VersionControlSystemDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class VersionControlSystemSynchronizer : IVersionControlSystemSynchronizer
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepositoryUpdater _repositoryUpdater;

    private readonly ICollectionSynchronizer<Repository, RepositoryDescriptor> _collectionSynchronizer;

    public VersionControlSystemSynchronizer(IRepositoryFactory repositoryFactory,
        IRepositoryUpdater repositoryUpdater,
        ICollectionSynchronizer<Repository, RepositoryDescriptor> collectionSynchronizer)
    {
        _repositoryFactory = repositoryFactory;
        _repositoryUpdater = repositoryUpdater;
        _collectionSynchronizer = collectionSynchronizer;
    }

    public void Update(VersionControlSystem target, VersionControlSystemDescriptor descriptor, IDependencyManager dependencyManager)
    {
        target.Location = descriptor.Location;
        UpdateRepositories(target, descriptor, dependencyManager);
    }

    private void UpdateRepositories(VersionControlSystem versionControlSystem,
        VersionControlSystemDescriptor changeDescriptor, IDependencyManager dependencyManager)
    {
        _collectionSynchronizer.Init(OnNew, OnUpdate, OnDelete);
        _collectionSynchronizer.Synchronize(versionControlSystem.Repositories, changeDescriptor.Repositories);
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