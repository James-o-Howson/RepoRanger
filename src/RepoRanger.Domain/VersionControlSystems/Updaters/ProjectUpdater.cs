using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Updaters;

public interface IProjectUpdater
{
    void Update(Project target, ProjectDescriptor descriptor, IDependencyManager dependencyManager);
}

internal sealed class ProjectUpdater : IProjectUpdater
{
    public void Update(Project target, ProjectDescriptor descriptor, IDependencyManager dependencyManager)
    {
        var metaData = descriptor.Metadata
            .Select(d => ProjectMetadata.Create(d.Key, d.Value))
            .ToList();
        
        target.Update(descriptor.Version, metaData, descriptor.Type);
        UpdateProjectDependencies(target, descriptor.ProjectDependencies, dependencyManager);
    }

    private void UpdateProjectDependencies(Project project,
        IReadOnlyCollection<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager)
    {
        foreach (var descriptor in descriptors)
        {
            var registrationResult = dependencyManager.Register(
                descriptor.Name, descriptor.Source, 
                descriptor.Version ?? string.Empty);
            
            
        }
    }
}