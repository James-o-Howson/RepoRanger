namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class RepositoryContext
{
    public string Name { get; init; } = string.Empty;
    public string RemoteUrl { get; init; } = string.Empty;
    public List<BranchContext> BranchContexts { get; set; } = [];

    public BranchContext DefaultBranch => BranchContexts.Single(b => b.IsDefault);
}