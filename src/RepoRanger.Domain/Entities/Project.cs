using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : Entity, IEquatable<Project>
{
    private readonly List<DependencyInstance> _dependencyInstances = [];
    private readonly List<Metadata> _metadata = [];
    
    private Project() { }
    
    public static Project Create(ProjectType type, string name, string version, string path, IReadOnlyCollection<Metadata>? metadata)
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
    
    public static Project Create(ProjectType type, string name, string version, string path, IReadOnlyCollection<Metadata>? metadata, 
        IEnumerable<DependencyInstance>? dependencyInstances)
    {
        var project = Create(type, name, version, path, metadata);
        if(dependencyInstances != null) project.AddDependencyInstances(dependencyInstances);
        
        return project;
    }

    public int Id { get; set; }
    public IReadOnlyCollection<Metadata> Metadata => _metadata;
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;
    public int RepositoryId { get; private set; }
    public Repository Repository { get; private set; } = null!;

    public void AddDependencyInstances(IEnumerable<DependencyInstance> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencyInstances.AddRange(dependencies.ToHashSet());
    }
    
    public void Update(string version, IEnumerable<Metadata> metadata, ProjectType type, 
        IReadOnlyCollection<DependencyInstance> dependencyInstances)
    {
        Version = version;
        
        _metadata.Clear();
        _metadata.AddRange(metadata);
        Type = type;
        
        var removed = DependencyInstances.Except(dependencyInstances);
        foreach (var dependencyInstance in removed)
        {
            _dependencyInstances.Remove(dependencyInstance);
        }
        
        var updated = dependencyInstances.Intersect(DependencyInstances);
        foreach (var dependencyInstance in updated)
        {
            var index = _dependencyInstances.IndexOf(dependencyInstance);
            _dependencyInstances[index].Update(dependencyInstance.Source, dependencyInstance.Version);
        }
        
        var added = dependencyInstances.Except(DependencyInstances);
        _dependencyInstances.AddRange(added);
    }
    
    internal void Delete()
    {
        _dependencyInstances.Clear();
    }
    
    public bool HasDependency(string dependencyName, string version) =>
        DependencyInstances.Any(di => di.DependencyName == dependencyName && di.Version == version);

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