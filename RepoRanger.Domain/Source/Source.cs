using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Source;

public sealed class Source : BaseCreatedAuditableEntity<Guid>
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

    public IReadOnlyCollection<Dependency> Dependencies() => 
        Repositories
            .SelectMany(r => r.Branches)
            .SelectMany(b => b.Projects)
            .SelectMany(p => p.Dependencies)
            .ToList();

    public IReadOnlyCollection<Project> Projects() => 
        Repositories
            .SelectMany(r => r.Branches)
            .SelectMany(b => b.Projects)
            .ToList();
    
    public IReadOnlyCollection<Branch> Branches() => 
        Repositories
            .SelectMany(r => r.Branches)
            .ToList();
    
    public void AddRepositories(IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        _repositories.AddRange(repositories);
    }
}