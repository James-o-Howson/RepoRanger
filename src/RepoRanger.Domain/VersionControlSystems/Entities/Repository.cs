using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class Repository : Entity
{
    private readonly List<Project> _projects = [];

    private Repository() {}
    
    public static Repository Create(VersionControlSystem vcs, string name, string remoteUrl, string defaultBranch)
    {
        var repository = new Repository
        {
            Name = name,
            RemoteUrl = remoteUrl,
            DefaultBranch = defaultBranch,
            VersionControlSystem = vcs,
            VersionControlSystemId = vcs.Id
        };
        
        return repository;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public Guid VersionControlSystemId { get; set; }
    public VersionControlSystem VersionControlSystem { get; private set; } = null!;
    public string DefaultBranch { get; set; } = string.Empty;
    public IReadOnlyCollection<Project> Projects => _projects;

    public IEnumerable<ProjectDependency> Dependencies => Projects
        .SelectMany(p => p.ProjectDependencies)
        .ToList();

    public void AddProject(Project project)
    {
        DomainException.ThrowIfNull(project);
        //todo: ensure unique projects
        _projects.Add(project);
    }
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        _projects.AddRange(projects.ToHashSet());
    }

    public void Update(string defaultBranch)
    {
        DefaultBranch = defaultBranch;
    }
    
    internal void Delete()
    {
        foreach (var branch in Projects)
        {
            branch.Delete();
        }

        _projects.Clear();
    }
    
    public void DeleteProject(Guid projectId)
    {
        var index = _projects.FindIndex(r => r.Id == projectId);
        if (index < 0) throw new DomainException($"Cannot delete Project with Id = {projectId} from Repository {Name}");
        
        _projects.RemoveAt(index);
    }
}