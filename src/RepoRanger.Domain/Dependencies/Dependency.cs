using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies;

public class Dependency : BaseAuditableEntity
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

    private bool HasVersion(DependencyVersionId versionId) => Versions.Any(v => v.Id == versionId);

    public void AddVulnerability(string osvId, string dependencyVersionValue, string dependencySourceValue)
    {
        var version = _versions.Single(v => v.Value == dependencyVersionValue);
        var source = _versions.SelectMany(v => v.Sources).First(s => s.Name == dependencySourceValue);
        if (version.HasVulnerability(osvId, source.Id)) return;
        
        var vulnerability = Vulnerability.Create(osvId, version.Id, source.Id);
        version.AddVulnerability(vulnerability);
        
        RaiseEvent(new DependencyVulnerableEvent(vulnerability.Id, version.Id, source.Id));
    }
}