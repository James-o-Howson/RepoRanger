using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Dependency : ICreatedAuditableEntity, IEquatable<Dependency>
{
    private readonly List<DependencyInstance> _dependencyInstances = [];
    
    public Dependency(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }
    
    public string Name { get; private set; }
    public IReadOnlyCollection<DependencyInstance> DependencyInstances => _dependencyInstances;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

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