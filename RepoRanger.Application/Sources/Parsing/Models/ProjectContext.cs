namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class ProjectContext
{
    public string Name { get; init; }
    public string Version { get; init; }
    public List<DependencyContext> DependencyContexts { get; set; } = [];
}