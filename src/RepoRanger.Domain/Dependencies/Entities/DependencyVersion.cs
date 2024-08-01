using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;

namespace RepoRanger.Domain.Dependencies.Entities;

public class DependencyVersion : Entity
{
    private readonly List<Vulnerability> _vulnerabilities = [];
    private readonly List<DependencySource> _sources = [];

    public Guid Id { get; } = Guid.NewGuid();
    public Guid DependencyId { get; private init; }
    public Dependency Dependency { get; private init; } = null!;
    public string Value { get; private init; } = null!;
    public IReadOnlyCollection<DependencySource> Sources => _sources;
    public IReadOnlyCollection<Vulnerability> Vulnerabilities => _vulnerabilities;

    private DependencyVersion() { }
    
    public static DependencyVersion Create(Dependency dependency, DependencySource source, string versionValue)
    {
        var version = new DependencyVersion
        {
            DependencyId = dependency.Id,
            Dependency = dependency,
            Value = versionValue
        };

        version.AddSource(source);
        source.AddVersion(version);
        
        return version;
    }

    public void AddVulnerability(Vulnerability vulnerability)
    {
        DomainException.ThrowIfNull(vulnerability);
        _vulnerabilities.Add(vulnerability);
    }

    public void AddSource(DependencySource source)
    {
        DomainException.ThrowIfNull(source);
        if (HasSource(source.Name)) return;
        _sources.Add(source);
        source.AddVersion(this);
    }

    private bool HasSource(string sourceName) => Sources.Any(s => s.Name == sourceName);
}