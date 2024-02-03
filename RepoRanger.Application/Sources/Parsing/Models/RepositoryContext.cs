namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class RepositoryContext
{
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string RemoteUrl { get; init; } = string.Empty;
    public List<BranchContext> BranchContexts { get; set; } = [];
}