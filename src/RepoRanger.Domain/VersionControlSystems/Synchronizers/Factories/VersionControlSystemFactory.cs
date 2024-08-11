using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;

internal interface IVersionControlSystemFactory
{
    VersionControlSystem Create(IDependencyManager dependencyManager, VersionControlSystemDescriptor descriptor);
}

internal sealed class VersionControlSystemFactory : IVersionControlSystemFactory
{
    private readonly IRepositoryFactory _repositoryFactory;

    public VersionControlSystemFactory(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public VersionControlSystem Create(IDependencyManager dependencyManager, VersionControlSystemDescriptor descriptor)
    {
        var vcs = VersionControlSystem.Create(descriptor.Name, descriptor.Location);
        vcs.AddRepositories(CreateRepositories(vcs, dependencyManager, descriptor));
        return vcs;
    }
    
    private IEnumerable<Repository> CreateRepositories(VersionControlSystem vcs, IDependencyManager dependencyManager,
        VersionControlSystemDescriptor descriptor) 
        => descriptor.Repositories.Select(d => _repositoryFactory.Create(vcs, d, dependencyManager));
}