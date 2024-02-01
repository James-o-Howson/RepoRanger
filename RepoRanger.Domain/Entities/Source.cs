using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Source : BaseAuditableEntity<Guid>
{
    private readonly List<Repository> _repositories = [];
    
    public string Name { get; private set; }
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    
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

    public void AddRepository(Repository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repositories.Add(repository);
    }
}