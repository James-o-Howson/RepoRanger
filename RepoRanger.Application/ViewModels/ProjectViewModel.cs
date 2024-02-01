namespace RepoRanger.Application.ViewModels;

public sealed record ProjectViewModel(string Name, string Version, IEnumerable<DependencyViewModel> Dependencies);