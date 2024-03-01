using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class DependencyInstance : ICreatedAuditableEntity
{
    private DependencyInstance() { }

    public DependencyInstance(DependencySource source, string dependencyName, string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(source);
        ArgumentException.ThrowIfNullOrEmpty(dependencyName);

        Id = Guid.NewGuid();
        Source = source;
        DependencyName = dependencyName;
        Version = Normalise(source, version);
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public DependencySource Source { get; private set; } = null!;
    public string DependencyName { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public Project Project { get; private set; } = null!;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    
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
}