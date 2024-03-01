using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.Entities;

public class Repository : ICreatedAuditableEntity
{
    private readonly List<Project> _projects = [];
    
    private Repository() { }
    
    public static Repository Create(string name, string remoteUrl, string defaultBranch)
    {
        var repository = new Repository
        {
            Name = name,
            RemoteUrl = remoteUrl,
            DefaultBranch = defaultBranch
        };
        
        return repository;
    }
    
    public static Repository Create(string name, string remoteUrl, string defaultBranch, 
        IEnumerable<Project> projects)
    {
        var repository = Create(name, remoteUrl, defaultBranch);
        repository.AddProjects(projects);
        
        return repository;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public int SourceId { get; set; }
    public Source Source { get; private set; } = null!;
    public string DefaultBranch { get; set; } = string.Empty;
    public IReadOnlyCollection<Project> Projects => _projects;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public IEnumerable<DependencyInstance> DependencyInstances => Projects
        .SelectMany(p => p.DependencyInstances)
        .ToList();
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        _projects.AddRange(projects);
    }
    
    internal void Delete()
    {
        foreach (var branch in Projects)
        {
            branch.Delete();
        }

        _projects.Clear();
    }
}