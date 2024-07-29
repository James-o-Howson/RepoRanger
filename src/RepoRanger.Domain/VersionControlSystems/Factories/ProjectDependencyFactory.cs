using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

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
        var (dependency, version, _) = dependencyManager.Register(
            descriptor.Name, descriptor.Source, 
            descriptor.Version ?? string.Empty);

        return ProjectDependency.Create(project, dependency, version);
    }
}