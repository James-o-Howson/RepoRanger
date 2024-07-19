namespace RepoRanger.Application.Contracts.Sources;

public sealed class SourceVm
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}