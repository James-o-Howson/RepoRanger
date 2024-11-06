using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.Events;
using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Domain.Exceptions;
using RepoRanger.SharedKernel.Base;

namespace RepoRanger.Domain.Dependencies;

public class Dependency : BaseAuditableEntity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public DependencyId Id { get; } = DependencyId.New;
    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<DependencyVersion> Versions => _versions;

    private Dependency() {}

    internal static Dependency Create(string name) => new()
    {
        Name = name
    };
    
    public void TryAddVersion(DependencyVersion version)
    {
        if (HasVersion(version.Id)) return;
        
        _versions.Add(version);
    }

    private bool HasVersion(DependencyVersionId versionId) => Versions.Any(v => v.Id == versionId);

    public void AddVulnerabilities(List<string> osvIds, string dependencyVersionValue, string dependencySourceValue)
    {
        if(osvIds.Count == 0) return;
        
        foreach (var osvId in osvIds)
        {
            var version = _versions.Single(v => v.Value == dependencyVersionValue);
            var source = _versions.SelectMany(v => v.Sources).First(s => s.Name == dependencySourceValue);
            if (version.HasVulnerability(osvId, source.Id)) return;
        
            var vulnerability = Vulnerability.Create(osvId, version.Id, source.Id);
            version.AddVulnerability(vulnerability);
        
            RaiseEvent(new DependencyVulnerabilityDiscovered(vulnerability.Id));
        }
    }
    
    public void DeleteVersion(DependencyVersionId versionId)
    {
        var index = _versions.FindIndex(v => v.Id == versionId);
        if (index < 0) throw new DomainException($"Cannot delete dependency version {versionId}");
        
        _versions.RemoveAt(index);
    }
}