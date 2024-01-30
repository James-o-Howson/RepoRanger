using RepoRanger.Domain.Common;
using RepoRanger.Domain.Exceptions;

namespace RepoRanger.Domain.ValueObjects;

public class Source : ValueObject
{
    static Source() { }
    
    private Source() { }

    public Source(string name)
    {
        Name = name;
    }
    
    public static Source From(string name)
    {
        var source = new Source
        {
            Name = name
        };

        if (!SupportedSources.Contains(source)) throw new UnsupportedSourceException(name);

        return source;
    }

    public static Source AzureDevOps = new("AzureDevOps");
    
    public static Source Gitolite = new("Gitolite");
    
    public string Name { get; private set; }
    
    
    public static implicit operator string(Source name)
    {
        return name.ToString();
    }

    public static explicit operator Source(string name)
    {
        return From(name);
    }

    public override string ToString()
    {
        return Name;
    }
    
    protected static IEnumerable<Source> SupportedSources
    {
        get
        {
            yield return AzureDevOps;
            yield return Gitolite;
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}