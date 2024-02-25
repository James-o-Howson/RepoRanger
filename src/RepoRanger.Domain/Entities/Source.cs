using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Source : ICreatedAuditableEntity
{
    private readonly List<Repository> _repositories = [];
    
    private Source() { }

    public Source(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    
    public IEnumerable<DependencyInstance> DependencyInstances => Repositories
        .SelectMany(r => r.DependencyInstances)
        .ToList();
    
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