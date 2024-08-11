using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;

internal interface IRepositoryFactory
{
    Repository Create(VersionControlSystem vcs, RepositoryDescriptor descriptor, IDependencyManager dependencyManager);
}

internal sealed class RepositoryFactory : IRepositoryFactory
{
    private readonly IProjectFactory _projectFactory;

    public RepositoryFactory(IProjectFactory projectFactory)
    {
        _projectFactory = projectFactory;
    }

    public Repository Create(VersionControlSystem vcs, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager)
    {
        var repository = Repository.Create(vcs, descriptor.Name, descriptor.RemoteUrl, descriptor.DefaultBranch);
        repository.AddProjects(CreateProjects(repository, descriptor, dependencyManager));
        
        return repository;
    }

    private List<Project> CreateProjects(Repository repository, RepositoryDescriptor descriptor, IDependencyManager dependencyManager) 
        => descriptor.Projects
            .Select(d => _projectFactory.Create(repository, d, dependencyManager))
            .ToList();
}