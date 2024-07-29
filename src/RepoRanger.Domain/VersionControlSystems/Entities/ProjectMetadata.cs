using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.VersionControlSystems.Entities;

public sealed class ProjectMetadata : Entity
{
    private ProjectMetadata() { }

    public static ProjectMetadata Create(string name, string value) => new()
    {
        Name = name,
        Value = value
    };

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; private init; } = string.Empty;
    public string Value { get; private init; } = string.Empty;
}