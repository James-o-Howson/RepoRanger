using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : ICreatedAuditableEntity
{
    private readonly List<DependencyInstance> _dependencyInstances = [];
    
    private Project() { }
    
    public static Project CreateNew(ProjectType type, string name, string version, params Metadata[] metadata) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Type = type,
        Version = version,
        Metadata = metadata.ToList()
    };
    
    public Guid Id { get; private set; } = Guid.Empty;
    public List<Metadata> Metadata { get; private set; } = []; 
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;
    public Guid RepositoryId { get; private set; } = Guid.Empty;
    public Repository Repository { get; private set; } = null!;
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