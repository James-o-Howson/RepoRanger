namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class RepositoryContext
{
    public string Name { get; init; }
    public string Url { get; init; }
    public string RemoteUrl { get; init; }
    public List<BranchContext> BranchContexts { get; set; } = [];
}