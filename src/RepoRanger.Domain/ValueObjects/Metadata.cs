using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.ValueObjects;

public sealed class Metadata : ValueObject
{
    private Metadata() { }
    
    public string Name { get; private init; } = string.Empty;
    public string Value { get; private init; } = string.Empty;
    
    public static Metadata Create(string name, string value)
    {
        var metadata = new Metadata
        {
            Name = name,
            Value = value
        };

        return metadata;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
    }
}