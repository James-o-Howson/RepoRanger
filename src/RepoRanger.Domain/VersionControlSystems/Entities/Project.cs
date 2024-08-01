using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class Project : Entity
{
    private readonly List<ProjectDependency> _projectDependencies = [];
    private readonly List<ProjectMetadata> _metadata = [];
    
    private Project() { }
    
    public static Project Create(Repository repository, ProjectType type, string name, string version, string path,
        IEnumerable<ProjectMetadata>? metadata)
    {
        var project = new Project
        {
            Name = name,
            Type = type,
            Version = version,
            Path = path,
            Repository = repository,
            RepositoryId = repository.Id
        };
        
        project._metadata.AddRange(metadata ?? []);
        
        return project;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public IReadOnlyCollection<ProjectMetadata> Metadata => _metadata;
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private init; } = string.Empty;
    public string Path { get; private init; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<ProjectDependency> ProjectDependencies => _projectDependencies;
    public Guid RepositoryId { get; private init; }
    public Repository Repository { get; private set; } = null!;

    public void AddDependencies(IEnumerable<ProjectDependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _projectDependencies.AddRange(dependencies.ToHashSet());
    }

    public void AddDependency(ProjectDependency projectDependency)
    {
        DomainException.ThrowIfNull(projectDependency);
        if (HasProjectDependency(projectDependency.Id)) return;
        _projectDependencies.Add(projectDependency);
    }

    private bool HasProjectDependency(Guid projectDependencyId)
    {
        DomainException.ThrowIfNull(projectDependencyId);
        return ProjectDependencies.Any(d => d.Id == projectDependencyId);
    }

    public void Update(string version, IEnumerable<ProjectMetadata> metadata, ProjectType type)
    {
        Version = version;
        
        _metadata.Clear();
        _metadata.AddRange(metadata);
        Type = type;
    }
    
    internal void Delete()
    {
        _projectDependencies.Clear();
    }

    public void DeleteProjectDependency(Guid projectDependencyId)
    {
        DomainException.ThrowIfNull(projectDependencyId);
        var index = _projectDependencies.FindIndex(d => d.Id == projectDependencyId);
        if (index < 0) return;

        _projectDependencies.RemoveAt(index);
    }
    
    public bool HasSpecificDependency(string name, string versionValue)
    {
        DomainException.ThrowIfNullOrEmpty(name);
        DomainException.ThrowIfNullOrEmpty(versionValue);
        
        return ProjectDependencies.Any(p => 
            p.Dependency.Name == name &&
            p.Version.Value == versionValue);
    }
}