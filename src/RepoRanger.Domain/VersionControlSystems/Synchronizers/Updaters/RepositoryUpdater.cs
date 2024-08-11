using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;

namespace RepoRanger.Domain.VersionControlSystems.Synchronizers.Updaters;

internal interface IRepositoryUpdater
{
    void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class RepositoryUpdater : IRepositoryUpdater
{
    private readonly IProjectFactory _projectFactory;
    private readonly IProjectUpdater _projectUpdater;

    public RepositoryUpdater(IProjectFactory projectFactory,
        IProjectUpdater projectUpdater)
    {
        _projectFactory = projectFactory;
        _projectUpdater = projectUpdater;
    }

    public void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager)
    {
        target.Update(descriptor.DefaultBranch);
        SynchronizeProjects(target, descriptor.Projects, dependencyManager);
    }
    
    private void SynchronizeProjects(Repository existingRepository, IReadOnlyCollection<ProjectDescriptor> projectDescriptors, IDependencyManager dependencyManager)
    {
        var synchronizer = new CollectionSynchronizer<Project, ProjectDescriptor>(OnNew, OnUpdate, OnDelete);
        synchronizer.Synchronize(existingRepository.Projects, projectDescriptors);
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