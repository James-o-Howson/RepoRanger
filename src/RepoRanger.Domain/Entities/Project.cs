using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : ICreatedAuditableEntity
{
    private readonly List<DependencyInstance> _dependencyInstances = [];
    
    private Project() { }

    public Project(ProjectType type, string name, string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(type);
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(version);
        
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
        Version = version;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public ProjectType Type { get; private set; }
    public string Version { get; private set; }
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;

    public Guid RepositoryId { get; private set; }
    public Repository Repository { get; private set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public void AddDependencies(IEnumerable<DependencyInstance> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencyInstances.AddRange(dependencies);
    }
    
    internal void Delete()
    {
        _dependencyInstances.Clear();
    }
}