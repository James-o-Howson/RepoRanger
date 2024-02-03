namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class BranchContext
{
    public string Name { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
    public List<ProjectContext> ProjectContexts { get; set; } = [];
}