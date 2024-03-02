using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Dependency : Auditable, IEquatable<Dependency>
{
    private readonly List<DependencyInstance> _dependencyInstances = [];

    private Dependency() { }
    
    public static Dependency CreateInstance(string name) => new() 
    {
        Name = name
    };

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;

    public void Delete()
    {
        _dependencyInstances.Clear();
    }

    public bool Equals(Dependency? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Dependency other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}