namespace RepoRanger.Application.Contracts.Sources;

public sealed class SourcePreviewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}