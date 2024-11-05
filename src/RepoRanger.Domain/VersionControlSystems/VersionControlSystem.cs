using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;
using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems;

public sealed class VersionControlSystem : BaseAuditableEntity, IAlternateIdProvider
{
    private readonly List<Repository> _repositories = [];
    
    private VersionControlSystem() { }

    public static VersionControlSystem Create(string name, string location)
    {
        var source = new VersionControlSystem
        {
            Name = name ?? throw new DomainException($"{nameof(name)} cannot be null"),
            Location = location ?? throw new DomainException($"{nameof(location)} cannot be null"),
        };
        
        return source;
    }

    public VersionControlSystemId Id { get; } = VersionControlSystemId.New;
    public string Name { get; private init; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    
    public IEnumerable<ProjectDependency> ProjectDependencies => Repositories
        .SelectMany(r => r.Dependencies)
        .ToList();
    
    public IEnumerable<string> DependencyNames => 
        ProjectDependencies.Select(d => d.Dependency.Name);
    
    public void AddRepository(Repository repository)
    {
        _repositories.Add(repository);
    }
    
    public void AddRepositories(IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        
        foreach (var repository in repositories)
        {
            AddRepository(repository);
        }
    }

    public void DeleteRepository(RepositoryId repositoryId)
    {
        var index = _repositories.FindIndex(r => r.Id == repositoryId);
        if (index < 0) throw new DomainException($"Deletion failed, cannot find Repository with Id {repositoryId} from VCS {Name}");

        _repositories[index].Delete();
        _repositories.RemoveAt(index);
    }

    public AlternateId GetAlternateId => new VersionControlSystemAlternateId(Name, Location);
}