using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies;

public class Dependency : Entity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public DependencyId Id { get; } = DependencyId.New;
    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyVersion> Versions => _versions;
    
    private Dependency() {}

    public static Dependency Create(string name) => new()
    {
        Name = name
    };
    
    public void TryAddVersion(DependencyVersion version)
    {
        DomainException.ThrowIfNull(version);
        if (HasVersion(version.Id)) return;
        
        _versions.Add(version);
    }

    public void AddVulnerability(DependencyVersionId versionId, Vulnerability vulnerability)
    {
        var version = Versions.FirstOrDefault(v => v.Id == versionId);
        if (version is null) throw new DomainException($"Cannot find {nameof(DependencyVersion)} for Id = {versionId}");
        version.AddVulnerability(vulnerability);
        
        RaiseEvent(new DependencyVulnerableEvent(Id));
    }

    private bool HasVersion(DependencyVersionId versionId) => Versions.Any(v => v.Id == versionId);
}