using RepoRanger.Domain.Exceptions;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;
using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;
using SharedKernel.Base;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public class Project : BaseAuditableEntity, IAlternateIdProvider
{
    private readonly List<ProjectDependency> _projectDependencies = [];
    private readonly List<ProjectMetadata> _metadata = [];
    
    private Project() { }
    
    public static Project Create(Repository repository, ProjectType type, string name, string version, string path,
        IEnumerable<ProjectMetadata>? metadata)
    {
        var project = new Project
        {
            Name = name,
            Type = type,
            Version = version,
            Path = path,
            Repository = repository,
            RepositoryId = repository.Id
        };
        
        project._metadata.AddRange(metadata ?? []);
        
        return project;
    }

    public ProjectId Id { get; } = ProjectId.New;
    public IReadOnlyCollection<ProjectMetadata> Metadata => _metadata;
    public ProjectType Type { get; private set; } = null!;
    public string Name { get; private init; } = string.Empty;
    public string Path { get; private init; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public IReadOnlyCollection<ProjectDependency> ProjectDependencies => _projectDependencies;
    public RepositoryId RepositoryId { get; private init; }
    public Repository Repository { get; private set; } = null!;

    public void AddProjectDependencies(IEnumerable<ProjectDependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _projectDependencies.AddRange(dependencies.ToHashSet());
    }

    public void AddProjectDependency(ProjectDependency projectDependency)
    {
        if (HasProjectDependency(projectDependency.Id)) return;
        _projectDependencies.Add(projectDependency);
    }

    public void AddMetadata(ProjectMetadata projectMetadata)
    {
        if (HasProjectMetadata(projectMetadata.Id)) return;
        _metadata.Add(projectMetadata);
    }

    public void Update(ProjectType type, string version, IEnumerable<ProjectMetadata> metadata)
    {
        Version = version;
        
        _metadata.Clear();
        _metadata.AddRange(metadata);
        Type = type;
    }

    internal void Delete()
    {
        _projectDependencies.Clear();
    }

    public void DeleteProjectDependency(ProjectDependencyId projectDependencyId)
    {
        var index = _projectDependencies.FindIndex(d => d.Id == projectDependencyId);
        if (index < 0) return;

        _projectDependencies.RemoveAt(index);
    }

    public void DeleteMetadata(ProjectMetadataId metadataId)
    {
        var index = _metadata.FindIndex(d => d.Id == metadataId);
        if (index < 0) return;

        _metadata.RemoveAt(index);
    }

    public bool HasSpecificDependency(string name, string versionValue)
    {
        DomainException.ThrowIfNullOrEmpty(name);
        DomainException.ThrowIfNullOrEmpty(versionValue);
        
        return ProjectDependencies.Any(p => 
            p.Dependency.Name == name &&
            p.Version.Value == versionValue);
    }

    public AlternateId GetAlternateId => new ProjectAlternateId(Name, Path);

    private bool HasProjectDependency(ProjectDependencyId projectDependencyId)
    {
        return ProjectDependencies.Any(d => d.Id == projectDependencyId);
    }

    private bool HasProjectMetadata(ProjectMetadataId metadataId)
    {
        return Metadata.Any(d => d.Id == metadataId);
    }
}