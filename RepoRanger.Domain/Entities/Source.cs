using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Source : BaseCreatedAuditableEntity<Guid>
{
    private readonly List<Repository> _repositories = [];
    
    public string Name { get; private set; }
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    
    public IEnumerable<Dependency> Dependencies => Repositories
        .SelectMany(r => r.Dependencies)
        .ToList();

    private Source() { }

    public Source(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Id = Guid.NewGuid();
        Name = name;
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
}