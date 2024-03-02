using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : ICreatedAuditableEntity
{
    private readonly List<DependencyInstance> _dependencyInstances = [];
    
    private Project() { }
    
    public static Project Create(ProjectType type, string name, string version, string path, IEnumerable<Metadata>? metadata)
    {
        var project = new Project
        {
            Name = name,
            Type = type,
            Version = version,
            Metadata = metadata?.ToList() ?? []
        };
        
        return project;
    }
    
    public static Project Create(ProjectType type, string name, string version, string path, IEnumerable<Metadata>? metadata, 
        IEnumerable<DependencyInstance>? dependencyInstances)
    {
        var project = Create(type, name, version, path, metadata);
        if(dependencyInstances != null) project.AddDependencyInstances(dependencyInstances);
        
        return project;
    }

    public int Id { get; set; }
    public List<Metadata> Metadata { get; private set; } = []; 
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Path { get; private set; }
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;
    public int RepositoryId { get; private set; }
    public Repository Repository { get; private set; } = null!;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public void AddDependencyInstances(IEnumerable<DependencyInstance> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencyInstances.AddRange(dependencies);
    }
    
    internal void Delete()
    {
        _dependencyInstances.Clear();
    }
}