using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Source;

public class Project : BaseCreatedAuditableEntity<Guid>
{
    private readonly List<Branch> _branches = [];
    private readonly List<Dependency> _dependencies = [];
    
    private Project() { }

    public Project(string name, string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(version);
        
        Id = Guid.NewGuid();
        Name = name;
        Version = version;
    }

    public string Name { get; private set; }
    public string Version { get; private set; }
    public IReadOnlyCollection<Branch> Branches => _branches;
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies;

    public void AddDependencies(IEnumerable<Dependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencies.AddRange(dependencies);
    }
}