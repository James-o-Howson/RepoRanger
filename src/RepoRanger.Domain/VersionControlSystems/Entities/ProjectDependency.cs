using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class ProjectDependency : Entity, IAlternateKeyProvider
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public Guid DependencyId { get; private set; }
    public Dependency Dependency { get; private set; } = null!;
    public Guid VersionId { get; private set; }
    public DependencyVersion Version { get; private set; } = null!;
    public Guid SourceId { get; private set; }
    public DependencySource Source { get; private set; } = null!;
    
    private ProjectDependency() { }

    public static ProjectDependency Create(Project project, Dependency dependency, DependencyVersion version, DependencySource source) => new()
    {
        ProjectId = project.Id,
        Project = project,
        DependencyId = dependency.Id,
        Dependency = dependency,
        VersionId = version.Id,
        Version = version,
        SourceId = source.Id,
        Source = source
    };

    public void Update(Dependency dependency, DependencyVersion version, DependencySource source)
    {
        DomainException.ThrowIfNull(dependency);
        DomainException.ThrowIfNull(version);
        DomainException.ThrowIfNull(source);

        Dependency = dependency;
        DependencyId = dependency.Id;
        Version = version;
        VersionId = version.Id;
        Source = source;
        SourceId = source.Id;
    }

    public AlternateKey GetAlternateKey => new ProjectDependencyAlternateKey(DependencyId, VersionId);
}