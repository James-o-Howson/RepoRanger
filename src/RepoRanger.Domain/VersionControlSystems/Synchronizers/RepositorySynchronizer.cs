using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers;

internal interface IRepositoryUpdater
{
    void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class RepositorySynchronizer : IRepositoryUpdater
{
    private readonly IProjectFactory _projectFactory;
    private readonly IProjectUpdater _projectUpdater;
    private readonly ICollectionSynchronizer<Project, ProjectDescriptor> _collectionSynchronizer;

    public RepositorySynchronizer(IProjectFactory projectFactory,
        IProjectUpdater projectUpdater,
        ICollectionSynchronizer<Project, ProjectDescriptor> collectionSynchronizer)
    {
        _projectFactory = projectFactory;
        _projectUpdater = projectUpdater;
        _collectionSynchronizer = collectionSynchronizer;
    }

    public void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager)
    {
        target.Update(descriptor.DefaultBranch);
        UpdateProjects(target, descriptor.Projects, dependencyManager);
    }
    
    private void UpdateProjects(Repository existingRepository, IReadOnlyCollection<ProjectDescriptor> projectDescriptors, IDependencyManager dependencyManager)
    {
        _collectionSynchronizer.Init(OnNew, OnUpdate, OnDelete);
        _collectionSynchronizer.Synchronize(existingRepository.Projects, projectDescriptors);
        return;
        
        void OnNew(ProjectDescriptor descriptor) => 
            existingRepository.AddProject(
                _projectFactory.Create(existingRepository, descriptor, dependencyManager));

        void OnUpdate(Project project, ProjectDescriptor descriptor) => 
            _projectUpdater.Update(project, descriptor, dependencyManager);

        void OnDelete(Project project) => 
            existingRepository.DeleteProject(project.Id);
    }
}