using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Branch : BaseCreatedAuditableEntity<Guid>
{
    private readonly List<Project> _projects = [];
    
    private Branch() { }

    public Branch(string name, bool isDefault)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(isDefault);
        
        Id = Guid.NewGuid();
        Name = name;
        IsDefault = isDefault;
    }

    public string Name { get; private set; }
    public bool IsDefault { get; private set; }
    public Guid RepositoryId { get; private set; }
    public IReadOnlyCollection<Project> Projects => _projects;

    public void AddProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);
        _projects.Add(project);
    }
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        
        _projects.AddRange(projects);
    }

    internal void Delete()
    {
        foreach (var project in Projects)
        {
            project.Delete();
        }
        
        _projects.Clear();
    }
}