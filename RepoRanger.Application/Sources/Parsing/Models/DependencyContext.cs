namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class DependencyContext
{
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
}