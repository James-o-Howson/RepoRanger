using RepoRanger.Application.Dependencies.Common;

namespace RepoRanger.Application.Projects.Common;

public sealed record ProjectDto(string Name, string Version, IEnumerable<DependencyDto> Dependencies);