namespace RepoRanger.Application.Sources.Common.Models;

public sealed record ProjectDto(string Name, string Version, IEnumerable<DependencyDto> Dependencies);