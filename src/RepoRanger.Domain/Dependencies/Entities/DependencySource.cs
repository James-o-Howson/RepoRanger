using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies.Entities;

public class DependencySource : BaseAuditableEntity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public DependencySourceId Id { get; } = DependencySourceId.New;
    public string Name { get; private init; } = null!;
    
    public IReadOnlyCollection<DependencyVersion> Versions => _versions;
    
    private DependencySource() {}

    public static DependencySource Create(string sourceName) => new()
    {
        Name = sourceName,
    };

    public void AddVersion(DependencyVersion version)
    {
        DomainException.ThrowIfNull(version);
        if (Versions.Any(v => v.Id == version.Id)) return;
        _versions.Add(version);
        version.TryAddSource(this);
    }
}