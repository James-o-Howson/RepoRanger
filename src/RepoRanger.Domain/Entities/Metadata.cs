using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Metadata : Entity
{
    private Metadata() { }

    public static Metadata Create(string name, string value) => new()
    {
        Name = name,
        Value = value
    };

    public int Id { get; set; }
    public string Name { get; private init; } = string.Empty;
    public string Value { get; private init; } = string.Empty;
}