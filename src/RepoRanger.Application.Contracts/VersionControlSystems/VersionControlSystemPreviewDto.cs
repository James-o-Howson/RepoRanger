namespace RepoRanger.Application.Contracts.VersionControlSystems;

public sealed class VersionControlSystemPreviewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}