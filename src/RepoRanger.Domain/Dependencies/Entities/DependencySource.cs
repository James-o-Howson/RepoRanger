using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Exceptions;

namespace RepoRanger.Domain.Dependencies.Entities;

public class DependencySource : Entity
{
    private readonly List<DependencyVersion> _versions = [];
    
    public Guid Id { get; } = Guid.NewGuid();
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
        version.AddSource(this);
    }
}