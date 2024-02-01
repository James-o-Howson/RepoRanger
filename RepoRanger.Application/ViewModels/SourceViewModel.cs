namespace RepoRanger.Application.ViewModels;

public sealed record SourceViewModel(string Name, IReadOnlyCollection<RepositoryViewModel> Repositories);