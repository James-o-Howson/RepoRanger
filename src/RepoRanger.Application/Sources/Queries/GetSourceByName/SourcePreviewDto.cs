namespace RepoRanger.Application.Sources.Queries.GetSourceByName;

public sealed class SourcePreviewDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}