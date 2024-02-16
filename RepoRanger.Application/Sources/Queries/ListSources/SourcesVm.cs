namespace RepoRanger.Application.Sources.Queries.ListSources;

public sealed class SourcesVm
{
    public IReadOnlyCollection<SourceVm> Sources { get; init; } = Array.Empty<SourceVm>();
}