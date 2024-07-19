namespace RepoRanger.Application.Contracts.Sources;

public sealed class SourcesVm
{
    public IReadOnlyCollection<SourceVm> Sources { get; init; } = Array.Empty<SourceVm>();
}