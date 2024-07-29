using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

internal interface IProjectDependencyFactory
{
    IEnumerable<ProjectDependency> Create(IEnumerable<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager);
}

internal sealed class ProjectDependencyFactory : IProjectDependencyFactory
{
    public IEnumerable<ProjectDependency> Create(IEnumerable<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager)
    {
        DomainException.ThrowIfNull(dependencyManager);
        return descriptors.Select(d => Create(d, dependencyManager));
    }
    
    private ProjectDependency Create(ProjectDependencyDescriptor descriptor, IDependencyManager dependencyManager)
    {
        var registrationResult = dependencyManager.Register(
            descriptor.Name, descriptor.Source, 
            descriptor.Version ?? string.Empty);
        
        var dependencyId = registrationResult.Dependency.Id;
        var versionId = registrationResult.Version.Id;

        return ProjectDependency.Create(dependencyId, versionId);
    }
}