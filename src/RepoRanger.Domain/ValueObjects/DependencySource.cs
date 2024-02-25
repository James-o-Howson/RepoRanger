using RepoRanger.Domain.Common;
using RepoRanger.Domain.Exceptions;

namespace RepoRanger.Domain.ValueObjects;

public sealed class DependencySource : ValueObject
{
    static DependencySource()
    {
    }

    private DependencySource()
    {
    }

    private DependencySource(string value)
    {
        Value = value;
    }

    public static DependencySource From(string value)
    {
        var source = new DependencySource { Value = value };

        if (!SupportedSources.Contains(source))
        {
            throw new UnsupportedDependencySourceException(value);
        }

        return source;
    }
    
    public string Value { get; init; }
    
    public static DependencySource Nuget => new("Nuget");
    public static DependencySource Npm => new("npm");
    public static DependencySource Local => new("Local");
    
    public static implicit operator string(DependencySource value)
    {
        return value.ToString();
    }

    public static explicit operator DependencySource(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }
    
    protected static IEnumerable<DependencySource> SupportedSources
    {
        get
        {
            yield return Nuget;
            yield return Npm;
            yield return Local;
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}