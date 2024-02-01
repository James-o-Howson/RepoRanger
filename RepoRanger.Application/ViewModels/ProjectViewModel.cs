namespace RepoRanger.Application.ViewModels;

public sealed record ProjectViewModel(string Name, string TypeName, string TypeVersion, List<DependencyViewModel> Dependencies);