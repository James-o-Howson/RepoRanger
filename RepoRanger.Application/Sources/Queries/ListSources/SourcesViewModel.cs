namespace RepoRanger.Application.Sources.Queries.ListSources;

public class SourcesViewModel
{
    public IReadOnlyCollection<SourceViewModel> Sources { get; init; } = Array.Empty<SourceViewModel>();
}