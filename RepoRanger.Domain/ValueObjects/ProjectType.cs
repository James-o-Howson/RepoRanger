using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.ValueObjects;

public class ProjectType : ValueObject
{
    static ProjectType()
    {
    } 
    
    private ProjectType()
    {
    }

    private ProjectType(string name, string version)
    {
        Name = name;
        Version = version;
    }
    
    public static ProjectType From(string name, string version)
    {
        var projectType = new ProjectType
        {
            Name = name, 
            Version = version
        };

        return projectType;
    }
    
    public string Name { get; private set; }
    public string Version { get; private set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Version;
    }
}