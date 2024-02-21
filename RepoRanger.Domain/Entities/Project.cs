using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Project : BaseCreatedAuditableEntity<Guid>
{
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
    public IReadOnlyCollection<Dependency> Dependencies => _dependencies;

    public Guid RepositoryId { get; private set; }
    public Repository Repository { get; private set; }

    public void AddDependencies(IEnumerable<Dependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _dependencies.AddRange(dependencies);
    }
    
    internal void Delete()
    {
        foreach (var dependency in _dependencies)
        {
            dependency.Delete();
        }
        
        _dependencies.Clear();
    }
}