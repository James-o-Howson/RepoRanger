using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Updaters;

public interface IRepositoryUpdater
{
    void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager);
}

internal sealed class RepositoryUpdater : IRepositoryUpdater
{
    private readonly IProjectFactory _projectFactory;
    private readonly IProjectUpdater _projectUpdater;

    public RepositoryUpdater(IProjectFactory projectFactory, IProjectUpdater projectUpdater)
    {
        _projectFactory = projectFactory;
        _projectUpdater = projectUpdater;
    }

    public void Update(Repository target, RepositoryDescriptor descriptor,
        IDependencyManager dependencyManager)
    {
        target.Update(descriptor.DefaultBranch);
        UpdateProjects(target, descriptor.Projects, dependencyManager);
    }
    
    private void UpdateProjects(Repository existingRepository, IReadOnlyCollection<ProjectDescriptor> projectDescriptors, IDependencyManager dependencyManager)
    {
        var existingProjects = existingRepository.Projects.ToDictionary(p => (p.Name, p.Path));
        var updatedProjects = projectDescriptors.ToDictionary(p => (p.Name, p.Path));

        // Update existing projects and create new ones
        foreach (var descriptor in projectDescriptors)
        {
            var key = (descriptor.Name, descriptor.Path);
            if (existingProjects.TryGetValue(key, out var existingProject))
            {
                _projectUpdater.Update(existingProject, descriptor, dependencyManager);
            }
            else
            {
                var project = _projectFactory.Create(existingRepository, descriptor, dependencyManager);
                existingRepository.AddProject(project);
            }
        }

        // Delete projects that are not present in the project descriptors
        var projectsToDelete = existingProjects.Values
            .Where(p => !updatedProjects.ContainsKey((p.Name, p.Path)))
            .ToList();

        foreach (var project in projectsToDelete)
        {
            existingRepository.DeleteProject(project.Id);
        }
    }
}