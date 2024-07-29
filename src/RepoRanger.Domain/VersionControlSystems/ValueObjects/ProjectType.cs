using RepoRanger.Domain.Common;
using RepoRanger.Domain.VersionControlSystems.Exceptions;

namespace RepoRanger.Domain.VersionControlSystems.ValueObjects;

public sealed class ProjectType : ValueObject
{
    private ProjectType()
    {
    }

    private ProjectType(string value)
    {
        Value = value;
    }

    public static ProjectType From(string value)
    {
        var source = new ProjectType { Value = value };

        if (!SupportedSources.Contains(source))
        {
            throw new UnsupportedProjectTypeException(value);
        }

        return source;
    }
    
    public string Value { get; init; } = null!;

    public static ProjectType Dotnet => new("Dotnet");
    public static ProjectType Angular => new("Angular");
    
    public static implicit operator string(ProjectType value)
    {
        return value.ToString();
    }

    public static explicit operator ProjectType(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }
    
    private static IEnumerable<ProjectType> SupportedSources
    {
        get
        {
            yield return Dotnet;
            yield return Angular;
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}