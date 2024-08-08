using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Domain.VersionControlSystems.Updaters;

internal interface IRepositoryUpdater
{
    void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class RepositoryUpdater : IRepositoryUpdater
{
    private readonly IProjectFactory _projectFactory;
    private readonly IProjectUpdater _projectUpdater;
    private readonly ICollectionSynchronizer<Project, ProjectDescriptor> _collectionSynchronizer;

    public RepositoryUpdater(IProjectFactory projectFactory,
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
        SynchronizeProjects(target, descriptor.Projects, dependencyManager);
    }
    
    private void SynchronizeProjects(Repository existingRepository, IReadOnlyCollection<ProjectDescriptor> projectDescriptors, IDependencyManager dependencyManager)
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