using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

internal interface IProjectFactory
{
    Project Create(Repository repository, ProjectDescriptor descriptor, IDependencyManager dependencyManager);
}

internal sealed class ProjectFactory : IProjectFactory
{
    private readonly IProjectMetadataFactory _projectMetadataFactory;
    private readonly IProjectDependencyFactory _projectDependencyFactory;

    public ProjectFactory(IProjectMetadataFactory projectMetadataFactory, IProjectDependencyFactory projectDependencyFactory)
    {
        _projectMetadataFactory = projectMetadataFactory;
        _projectDependencyFactory = projectDependencyFactory;
    }

    public Project Create(Repository repository, ProjectDescriptor descriptor, IDependencyManager dependencyManager)
    {
        var metaData = _projectMetadataFactory.Create(descriptor.Metadata);
        var project = Project.Create(repository, descriptor.Type, descriptor.Name, descriptor.Version,
            descriptor.Path, metaData);
        
        project.AddDependencies(_projectDependencyFactory.Create(project, descriptor.ProjectDependencies, dependencyManager));

        return project;
    }
}