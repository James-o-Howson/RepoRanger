using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Repository : Auditable, IEquatable<Repository>
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

    public bool IsNew => Id == 0;

    public IEnumerable<DependencyInstance> DependencyInstances => Projects
        .SelectMany(p => p.DependencyInstances)
        .ToList();
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        _projects.AddRange(projects);
    }

    public void Update(string defaultBranch, IReadOnlyCollection<Project> projects)
    {
        DefaultBranch = defaultBranch;
        
        var removed = Projects.Except(projects);
        foreach (var project in removed)
        {
            _projects.Remove(project);
        }
        
        var updated = projects.Intersect(Projects);
        foreach (var project in updated)
        {
            var index = _projects.IndexOf(project);
            _projects[index].Update(project.Version, project.Metadata, project.Type, project.DependencyInstances);
        }
        
        var added = projects.Except(Projects);
        _projects.AddRange(added);
    }
    
    
    internal void Delete()
    {
        foreach (var branch in Projects)
        {
            branch.Delete();
        }

        _projects.Clear();
    }

    public bool Equals(Repository? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && RemoteUrl == other.RemoteUrl && SourceId == other.SourceId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Repository)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, RemoteUrl, SourceId);
    }
}