namespace RepoRanger.Application.Sources.Queries.GetSourceDetails;

public sealed class SourceDetailsVm
{
    public IReadOnlyCollection<SourceDetailVm> Sources { get; init; } = Array.Empty<SourceDetailVm>();
}