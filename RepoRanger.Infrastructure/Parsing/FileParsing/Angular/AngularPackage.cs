namespace RepoRanger.Infrastructure.Parsing.FileParsing.Angular;

internal sealed class PackageJson
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public Dictionary<string, string> Scripts { get; set; } = new();
    public bool Private { get; set; }
    public Dictionary<string, string?> Dependencies { get; set; } = new();
    public Dictionary<string, string> DevDependencies { get; set; } = new();
}