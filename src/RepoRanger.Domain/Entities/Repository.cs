using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.Entities;

public class Repository : ICreatedAuditableEntity
{
    private readonly List<Project> _projects = [];
    
    private Repository() { }
    
    public static Repository Create(string name, string remoteUrl, string defaultBranch, Guid sourceId) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        RemoteUrl = remoteUrl,
        DefaultBranch = defaultBranch,
        SourceId = sourceId
    };
    
    public Guid Id { get; private set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public Guid SourceId { get; private set; } = Guid.Empty;
    public Source Source { get; private set; } = null!;
    public string DefaultBranch { get; set; } = string.Empty;
    public IReadOnlyCollection<Project> Projects => _projects;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public IEnumerable<DependencyInstance> DependencyInstances => Projects
        .SelectMany(p => p.DependencyInstances)
        .ToList();
    
    public void AddProjects(List<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        if (projects.Count == 0) throw new ArgumentException("Cannot be empty.");
        
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