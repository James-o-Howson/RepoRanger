using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Contracts;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Updaters;

internal interface IProjectUpdater
{
    void Update(Project target, ProjectDescriptor descriptor, IDependencyManager dependencyManager);
}

internal sealed class ProjectUpdater : IProjectUpdater
{
    private readonly ICollectionSynchronizer<ProjectDependency, RegistrationResult> _projectDependencySynchronizer;
    private readonly ICollectionSynchronizer<ProjectMetadata, ProjectMetadataDescriptor> _projectMetadataSynchronizer;

    public ProjectUpdater(ICollectionSynchronizer<ProjectDependency, RegistrationResult> projectDependencySynchronizer,
        ICollectionSynchronizer<ProjectMetadata, ProjectMetadataDescriptor> projectMetadataSynchronizer)
    {
        _projectDependencySynchronizer = projectDependencySynchronizer;
        _projectMetadataSynchronizer = projectMetadataSynchronizer;
    }

    public void Update(Project target, ProjectDescriptor descriptor, IDependencyManager dependencyManager)
    {
        var metaData = descriptor.Metadata
            .Select(d => ProjectMetadata.Create(d.Key, d.Value))
            .ToList();
        
        target.Update(descriptor.Type, descriptor.Version, metaData);
        SynchronizeProjectDependencies(target, descriptor.ProjectDependencies, dependencyManager);
        SynchronizeProjectMetadata(target, descriptor.Metadata);
    }

    private void SynchronizeProjectMetadata(Project project, IReadOnlyCollection<ProjectMetadataDescriptor> descriptorMetadata)
    {
        _projectMetadataSynchronizer.Init(OnNew, OnUpdate, OnDelete);
        _projectMetadataSynchronizer.Synchronize(project.Metadata, descriptorMetadata);
        return;
        
        void OnNew(ProjectMetadataDescriptor descriptor) => 
            project.AddMetadata(ProjectMetadata.Create(descriptor.Key, descriptor.Value));

        void OnUpdate(ProjectMetadata metadata, ProjectMetadataDescriptor descriptor) => 
            metadata.Update(descriptor.Value);

        void OnDelete(ProjectMetadata metadata) => 
            project.DeleteMetadata(metadata.Id);
    }

    private void SynchronizeProjectDependencies(Project project,
        IReadOnlyCollection<ProjectDependencyDescriptor> descriptors,
        IDependencyManager dependencyManager)
    {
        var registrationResults = RegisterDependencies(descriptors, dependencyManager);

        _projectDependencySynchronizer.Init(OnNew, OnUpdate, OnDelete);
        _projectDependencySynchronizer.Synchronize(project.ProjectDependencies, registrationResults);
        return;
        
        void OnNew(RegistrationResult registrationResult) => 
            project.AddProjectDependency(ProjectDependency.Create(project, 
                registrationResult.Dependency,
                registrationResult.Version,
                registrationResult.Source));

        void OnUpdate(ProjectDependency projectDependency, RegistrationResult registrationResult) => 
            projectDependency.Update(registrationResult.Dependency, 
                registrationResult.Version, 
                registrationResult.Source);

        void OnDelete(ProjectDependency projectDependency) => 
            project.DeleteProjectDependency(projectDependency.Id);
    }
    
    private static List<RegistrationResult> RegisterDependencies(IEnumerable<ProjectDependencyDescriptor> descriptors, IDependencyManager dependencyManager) =>
        descriptors
            .Select(d => RegisterDependency(dependencyManager, d))
            .ToList();

    private static RegistrationResult RegisterDependency(IDependencyManager dependencyManager, ProjectDependencyDescriptor descriptor) =>
        dependencyManager.Register(
            descriptor.Name, descriptor.Source, 
            descriptor.Version ?? string.Empty);
}