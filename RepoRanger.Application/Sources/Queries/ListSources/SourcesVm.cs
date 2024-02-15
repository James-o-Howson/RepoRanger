namespace RepoRanger.Application.Sources.Queries.ListSources;

public class SourcesVm
{
    public IReadOnlyCollection<SourceVm> Sources { get; init; } = Array.Empty<SourceVm>();
}