using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : BaseAuditableEntity<Guid>
{
    private readonly List<Branch> _branches = [];
    private readonly List<Dependency> _dependencies = [];
    
    private Project() { }

    public Project(string name, ProjectType type)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(type);
        
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }

    public string Name { get; private set; }
    public ProjectType Type { get; private set; }
    public IReadOnlyCollection<Branch> Branches => _branches;
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies;

    public void AddDependencies(IEnumerable<Dependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencies.AddRange(dependencies);
    }
}