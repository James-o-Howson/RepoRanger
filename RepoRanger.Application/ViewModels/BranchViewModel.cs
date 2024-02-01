namespace RepoRanger.Application.ViewModels;

public sealed record BranchViewModel(string Name, bool IsDefault, List<ProjectViewModel> Projects);