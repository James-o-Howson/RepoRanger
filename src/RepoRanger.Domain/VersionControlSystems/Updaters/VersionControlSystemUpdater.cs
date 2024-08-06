using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Updaters;

public interface IVersionControlSystemUpdater
{
    void Update(VersionControlSystem target, VersionControlSystemDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class VersionControlSystemUpdater : IVersionControlSystemUpdater
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepositoryUpdater _repositoryUpdater;

    public VersionControlSystemUpdater(IRepositoryFactory repositoryFactory, IRepositoryUpdater repositoryUpdater)
    {
        _repositoryFactory = repositoryFactory;
        _repositoryUpdater = repositoryUpdater;
    }

    public void Update(VersionControlSystem target, VersionControlSystemDescriptor descriptor, IDependencyManager dependencyManager)
    {
        target.Location = descriptor.Location;
        UpdateRepositories(target, descriptor, dependencyManager);
    }

    private void UpdateRepositories(VersionControlSystem versionControlSystem,
        VersionControlSystemDescriptor changeDescriptor, IDependencyManager dependencyManager)
    {
        var existingRepositories = versionControlSystem.Repositories.ToDictionary(r => (r.Name, r.RemoteUrl));
        var updatedRepositories = changeDescriptor.Repositories.ToDictionary(r => (r.Name, r.RemoteUrl));

        // Update existing repositories and create new ones
        foreach (var descriptor in changeDescriptor.Repositories)
        {
            var key = (descriptor.Name, descriptor.RemoteUrl);
            if (existingRepositories.TryGetValue(key, out var existingRepository))
            {
                _repositoryUpdater.Update(existingRepository, descriptor, dependencyManager);
            }
            else
            {
                var repository = _repositoryFactory.Create(versionControlSystem, descriptor, dependencyManager);
                versionControlSystem.AddRepository(repository);
            }
        }

        // Delete repositories that are not present in the change descriptor
        var repositoriesToDelete = existingRepositories.Values
            .Where(r => !updatedRepositories.ContainsKey((r.Name, r.RemoteUrl)))
            .ToList();

        foreach (var repository in repositoriesToDelete)
        {
            versionControlSystem.DeleteRepository(repository.Id);
        }
    }
}