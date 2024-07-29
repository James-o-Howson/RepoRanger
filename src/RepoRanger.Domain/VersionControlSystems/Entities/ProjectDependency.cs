using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class ProjectDependency : Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public Guid DependencyId { get; private set; }
    public Dependency Dependency { get; private set; } = null!;
    public Guid VersionId { get; private set; }
    public DependencyVersion Version { get; private set; } = null!;
    
    private ProjectDependency() { }

    public static ProjectDependency Create(Project project, Dependency dependency, DependencyVersion version) => new()
    {
        ProjectId = project.Id,
        Project = project,
        DependencyId = dependency.Id,
        Dependency = dependency,
        VersionId = version.Id,
        Version = version
    };

    public void Update(Dependency dependency, DependencyVersion version)
    {
        DomainException.ThrowIfNull(dependency);
        DomainException.ThrowIfNull(version);

        Dependency = dependency;
        DependencyId = dependency.Id;
        Version = version;
        VersionId = version.Id;
    }
}