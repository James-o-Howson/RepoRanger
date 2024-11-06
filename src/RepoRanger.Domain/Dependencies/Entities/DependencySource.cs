using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.SharedKernel.Base;

namespace RepoRanger.Domain.Dependencies.Entities;

public class DependencySource : BaseAuditableEntity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public DependencySourceId Id { get; } = DependencySourceId.New;
    public DependencySourceName Name { get; private init; } = null!;
    
    public IReadOnlyCollection<DependencyVersion> Versions => _versions;
    
    private DependencySource() {}

    internal static DependencySource Create(DependencySourceName sourceName) => new()
    {
        Name = sourceName,
    };

    public void AddVersion(DependencyVersion version)
    {
        if (Versions.Any(v => v.Id == version.Id)) return;
        _versions.Add(version);
        version.TryAddSource(this);
    }
}