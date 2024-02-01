namespace RepoRanger.Application.ViewModels;

public sealed record RepositoryViewModel(string Name, string Url, string RemoteUrl, List<BranchViewModel> Branches);