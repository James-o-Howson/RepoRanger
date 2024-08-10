using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Entities;

public class DependencyVersion : BaseAuditableEntity
{
    private readonly List<Vulnerability> _vulnerabilities = [];
    private readonly List<DependencySource> _sources = [];

    public DependencyVersionId Id { get; } = DependencyVersionId.New;
    public DependencyId DependencyId { get; private init; }
    public Dependency Dependency { get; private init; } = null!;
    public string? Value { get; private init; }
    public IReadOnlyCollection<DependencySource> Sources => _sources;
    public IReadOnlyCollection<Vulnerability> Vulnerabilities => _vulnerabilities;

    private DependencyVersion() { }
    
    public static DependencyVersion Create(Dependency dependency, DependencySource source, string? versionValue)
    {
        var version = new DependencyVersion
        {
            DependencyId = dependency.Id,
            Dependency = dependency,
            Value = versionValue
        };

        version.TryAddSource(source);
        source.AddVersion(version);
        
        return version;
    }

    public void AddVulnerability(Vulnerability vulnerability)
    {
        DomainException.ThrowIfNull(vulnerability);
        _vulnerabilities.Add(vulnerability);
    }

    public void TryAddSource(DependencySource source)
    {
        DomainException.ThrowIfNull(source);
        if (HasSource(source.Name)) return;
        _sources.Add(source);
        source.AddVersion(this);
    }

    private bool HasSource(string sourceName) => Sources.Any(s => s.Name == sourceName);
}