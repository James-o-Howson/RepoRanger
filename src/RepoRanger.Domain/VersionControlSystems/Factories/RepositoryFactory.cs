using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

public interface IRepositoryFactory
{
    Repository Create(RepositoryDescriptor descriptor, IDependencyManager dependencyManager);
}

internal sealed class RepositoryFactory : IRepositoryFactory
{
    private readonly IProjectFactory _projectFactory;

    public RepositoryFactory(IProjectFactory projectFactory)
    {
        _projectFactory = projectFactory;
    }

    public Repository Create(RepositoryDescriptor descriptor, IDependencyManager dependencyManager)
    {
        var repository = Repository.Create(descriptor.Name, descriptor.RemoteUrl, descriptor.DefaultBranch);
        repository.AddProjects(CreateProjects(descriptor, dependencyManager));
        
        return repository;
    }

    private List<Project> CreateProjects(RepositoryDescriptor descriptor, IDependencyManager dependencyManager) 
        => descriptor.Projects
            .Select(d => _projectFactory.Create(d, dependencyManager))
            .ToList();
}