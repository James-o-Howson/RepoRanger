using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;

internal interface IProjectDependencyFactory
{
    IEnumerable<ProjectDependency> Create(Project project, IEnumerable<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager);
}

internal sealed class ProjectDependencyFactory : IProjectDependencyFactory
{
    public IEnumerable<ProjectDependency> Create(Project project, IEnumerable<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager)
    {
        DomainException.ThrowIfNull(dependencyManager);
        return descriptors.Select(d => Create(project, d, dependencyManager));
    }
    
    private static ProjectDependency Create(Project project, ProjectDependencyDescriptor descriptor,
        IDependencyManager dependencyManager)
    {
        var (dependency, version, source) = dependencyManager.Register(
            descriptor.Name, descriptor.Source, 
            descriptor.Version);

        return ProjectDependency.Create(project, dependency, version, source);
    }
}