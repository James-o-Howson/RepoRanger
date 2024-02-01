namespace RepoRanger.Application.ViewModels;

public sealed record SourceViewModel(string Name, IEnumerable<RepositoryViewModel> Repositories);