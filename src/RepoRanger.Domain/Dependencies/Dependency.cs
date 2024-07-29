using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.Events;

namespace RepoRanger.Domain.Dependencies;

public class Dependency : Entity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyVersion> Versions => _versions;
    
    private Dependency() {}

    public static Dependency Create(string name) => new()
    {
        Name = name
    };
    
    public void AddVersion(DependencyVersion version)
    {
        DomainException.ThrowIfNull(version);
        if (HasVersion(version.Id)) return;
        
        _versions.Add(version);
    }

    public void AddVulnerability(Guid versionId, Vulnerability vulnerability)
    {
        var version = Versions.FirstOrDefault(v => v.Id == versionId);
        if (version is null) throw new DomainException($"Cannot find {nameof(DependencyVersion)} for Id = {versionId}");
        version.AddVulnerability(vulnerability);
        
        RaiseEvent(new DependencyVulnerableEvent(Id));
    }

    public bool HasVersion(Guid versionId)
    {
        DomainException.ThrowIfNull(versionId);
        return Versions.Any(v => v.Id == versionId);
    }
}