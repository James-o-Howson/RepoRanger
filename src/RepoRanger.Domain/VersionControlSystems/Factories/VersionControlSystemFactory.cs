using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

public interface IVersionControlSystemFactory
{
    VersionControlSystem Create(IDependencyManager dependencyManager, VersionControlSystemDescriptor descriptor);
}

public class VersionControlSystemFactory : IVersionControlSystemFactory
{
    private readonly IRepositoryFactory _repositoryFactory;

    public VersionControlSystemFactory(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public VersionControlSystem Create(IDependencyManager dependencyManager, VersionControlSystemDescriptor descriptor)
    {
        var vcs = VersionControlSystem.Create(descriptor.Name, descriptor.Location);
        vcs.AddRepositories(CreateRepositories(dependencyManager, descriptor));
        return vcs;
    }
    
    private IEnumerable<Repository> CreateRepositories(IDependencyManager dependencyManager, VersionControlSystemDescriptor descriptor) 
        => descriptor.Repositories.Select(d => _repositoryFactory.Create(d, dependencyManager));
}