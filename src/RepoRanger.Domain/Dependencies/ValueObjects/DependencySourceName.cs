using RepoRanger.Domain.Exceptions;
using SharedKernel.Base;

namespace RepoRanger.Domain.Dependencies.ValueObjects;

public sealed class DependencySourceName : ValueObject
{
    private DependencySourceName()
    {
    }

    private DependencySourceName(string value)
    {
        Value = value;
    }

    public static DependencySourceName From(string value)
    {
        var source = new DependencySourceName { Value = value };

        if (!SupportedSources.Contains(source))
        {
            throw new DomainException($"Dependency Source Name \"{value}\" is unsupported.");
        }

        return source;
    }
    
    public string Value { get; init; } = null!;

    public static DependencySourceName Npm => new("npm");
    public static DependencySourceName Nuget => new("NuGet");
    public static DependencySourceName LocalReference => new("LocalReference");
    
    public static implicit operator string(DependencySourceName value)
    {
        return value.ToString();
    }

    public static explicit operator DependencySourceName(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }
    
    private static IEnumerable<DependencySourceName> SupportedSources
    {
        get
        {
            yield return Npm;
            yield return Nuget;
            yield return LocalReference;
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}