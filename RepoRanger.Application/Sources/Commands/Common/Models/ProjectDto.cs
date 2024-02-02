namespace RepoRanger.Application.Sources.Commands.Common.Models;

public sealed record ProjectDto(string Name, string Version, IEnumerable<DependencyDto> Dependencies);