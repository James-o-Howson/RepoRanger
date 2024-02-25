namespace RepoRanger.Application.Sources.Queries.ListSources;

public sealed class SourceVm
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}