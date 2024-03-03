using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class DependencyInstance : Auditable, IEquatable<DependencyInstance>
{
    private DependencyInstance() { }

    public static DependencyInstance Create(DependencySource source, string dependencyName, string version) => new()
    {
        Source = source,
        DependencyName = dependencyName,
        Version = Normalise(source, version)
    };

    public int Id { get; set; }
    public DependencySource Source { get; private set; } = null!;
    public string DependencyName { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    
    public void Update(DependencySource source, string version)
    {
        Source = source;
        Version = version;
    }

    private static string Normalise(string source, string version)
    {
        if (source != "Nuget") return version;
        
        var parts = version.Split('.');

        // Remove trailing zeros from version parts
        var lastNonZeroIndex = Array.FindLastIndex(parts, p => p != "0");
        var normalizedParts = new string[lastNonZeroIndex + 1];
        Array.Copy(parts, normalizedParts, lastNonZeroIndex + 1);

        return string.Join(".", normalizedParts);
    }

    public bool Equals(DependencyInstance? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DependencyName == other.DependencyName && ProjectId == other.ProjectId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DependencyInstance)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DependencyName, ProjectId);
    }
}