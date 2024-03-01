namespace RepoRanger.Application.Sources.Queries.GetByName;

public sealed class SourcePreviewDto
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}