namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class ProjectContext
{
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public List<DependencyContext> DependencyContexts { get; set; } = [];
}