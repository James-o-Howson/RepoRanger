using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class Repository : Entity, IAlternateKeyProvider
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

    public RepositoryId Id { get; } = RepositoryId.New;
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public VersionControlSystemId VersionControlSystemId { get; set; }
    public VersionControlSystem VersionControlSystem { get; private set; } = null!;
    public string DefaultBranch { get; set; } = string.Empty;
    public IReadOnlyCollection<Project> Projects => _projects;

    public IEnumerable<ProjectDependency> Dependencies => Projects
        .SelectMany(p => p.ProjectDependencies)
        .ToList();

    public bool HasProject(Project project)
    {
        DomainException.ThrowIfNull(project);
        return _projects.FindIndex(p => p.Id == project.Id) >= 0;
    }
    
    public void AddProject(Project project)
    {
        DomainException.ThrowIfNull(project);
        if (HasProject(project))
            throw new DomainException($"Repository: Id={Id} Name={Name} already contains Project: Id={project.Id} Name={project.Name} Path={project.Path}");
        
        _projects.Add(project);
    }
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);

        foreach (var project in projects)
        {
            AddProject(project);
        }
    }

    public void Update(string defaultBranch)
    {
        DomainException.ThrowIfNullOrEmpty(defaultBranch);
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
    
    public void DeleteProject(ProjectId projectId)
    {
        var index = _projects.FindIndex(r => r.Id == projectId);
        if (index < 0) throw new DomainException($"Cannot delete Project with Id = {projectId} from Repository {Name}");
        
        _projects.RemoveAt(index);
    }

    public AlternateKey GetAlternateKey => new RepositoryAlternateKey(Name, RemoteUrl);
}