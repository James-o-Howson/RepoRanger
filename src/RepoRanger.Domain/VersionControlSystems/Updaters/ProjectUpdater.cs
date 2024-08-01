using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Contracts;
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
        var dependencyRegistrations = descriptors
            .Select(d => RegisterDependency(dependencyManager, d))
            .ToList();
        
        var existingCompositeKeys = project.ProjectDependencies
            .ToDictionary(d => (d.DependencyId, d.VersionId));
        var updatedCompositeKeys = dependencyRegistrations.ToDictionary(r => (r.Dependency.Id, r.Version.Id));

        foreach (var dependencyRegistration in dependencyRegistrations)
        {
            var compositeKey = (dependencyRegistration.Dependency.Id, dependencyRegistration.Version.Id);
            if (existingCompositeKeys.TryGetValue(compositeKey, out var existingProjectDependency))
            {
                existingProjectDependency.Update(dependencyRegistration.Dependency, 
                    dependencyRegistration.Version, 
                    dependencyRegistration.Source);
            }
            else
            {
                var projectDependency = ProjectDependency.Create(project, 
                    dependencyRegistration.Dependency,
                    dependencyRegistration.Version,
                    dependencyRegistration.Source);
                
                project.AddDependency(projectDependency);
            }
        }
        
        // Delete projects that are not present in the project descriptors
        var projectDependenciesToDelete = existingCompositeKeys.Values
            .Where(p => !updatedCompositeKeys.ContainsKey((p.DependencyId, p.VersionId)))
            .ToList();

        foreach (var projectDependency in projectDependenciesToDelete)
        {
            project.DeleteProjectDependency(projectDependency.Id);
        }
    }

    private static RegistrationResult RegisterDependency(IDependencyManager dependencyManager, ProjectDependencyDescriptor descriptor) =>
        dependencyManager.Register(
            descriptor.Name, descriptor.Source, 
            descriptor.Version ?? string.Empty);
}