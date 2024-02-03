namespace RepoRanger.Infrastructure.Parsing.FileParsing.Angular;

internal sealed class PackageJson
{
    public string Name { get; set; }
    public string Version { get; set; }
    public Dictionary<string, string> Scripts { get; set; }
    public bool Private { get; set; }
    public Dictionary<string, string?> Dependencies { get; set; }
    public Dictionary<string, string> DevDependencies { get; set; }
}