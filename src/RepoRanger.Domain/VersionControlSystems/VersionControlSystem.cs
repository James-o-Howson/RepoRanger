using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Domain.VersionControlSystems;

public sealed class VersionControlSystem : Entity, IEquatable<VersionControlSystem>
{
    private readonly List<Repository> _repositories = [];
    
    private VersionControlSystem() { }

    public static VersionControlSystem Create(string name, string location)
    {
        var source = new VersionControlSystem
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Cannot be null"),
            Location = location ?? throw new ArgumentNullException(nameof(location), "Cannot be null")
        };
        
        return source;
    }
    
    public static VersionControlSystem Create(string name, string location, IEnumerable<Repository> repositories)
    {
        var source = Create(name, location);
        source.AddRepositories(repositories);
        return source;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; private init; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    
    public IEnumerable<ProjectDependency> Dependencies => Repositories
        .SelectMany(r => r.Dependencies)
        .ToList();
    
    public IEnumerable<string> DependencyNames => 
        Dependencies.Select(d => d.Dependency.Name);
    
    public void AddRepository(Repository repository)
    {
        DomainException.ThrowIfNull(repository);
        _repositories.Add(repository);
    }
    
    public void AddRepositories(IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        _repositories.AddRange(repositories);
    }
    
    public void Delete()
    {
        foreach (var repository in Repositories)
        {
            repository.Delete();
        }

        _repositories.Clear();
    }

    public void DeleteRepository(Guid repositoryId)
    {
        var index = _repositories.FindIndex(r => r.Id == repositoryId);
        if (index < 0) throw new DomainException($"Cannot delete Repository with Id = {repositoryId} from VCS {Name}");
        
        _repositories.RemoveAt(index);
    }

    public bool Equals(VersionControlSystem? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is VersionControlSystem other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}