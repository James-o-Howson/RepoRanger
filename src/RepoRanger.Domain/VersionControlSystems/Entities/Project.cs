using RepoRanger.Domain.Common;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class Project : Entity, IEquatable<Project>
{
    private readonly List<ProjectDependency> _projectDependencies = [];
    private readonly List<ProjectMetadata> _metadata = [];
    
    private Project() { }
    
    public static Project Create(ProjectType type, string name, string version, string path, IEnumerable<ProjectMetadata>? metadata)
    {
        var project = new Project
        {
            Name = name,
            Type = type,
            Version = version,
            Path = path
        };
        
        project._metadata.AddRange(metadata ?? []);
        
        return project;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public IReadOnlyCollection<ProjectMetadata> Metadata => _metadata;
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<ProjectDependency> ProjectDependencies => _projectDependencies;
    public Guid RepositoryId { get; private set; }
    public Repository Repository { get; private set; } = null!;

    public void AddDependencies(IEnumerable<ProjectDependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _projectDependencies.AddRange(dependencies.ToHashSet());
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
    
    public bool Equals(Project? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Path == other.Path && RepositoryId == other.RepositoryId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Project)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Path, RepositoryId);
    }
}