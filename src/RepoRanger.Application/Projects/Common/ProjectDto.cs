using RepoRanger.Application.DependencyInstances.Common;

namespace RepoRanger.Application.Projects.Common;

public sealed record ProjectDto(string Type, string Name, string Version, string Path, IEnumerable<DependencyInstanceDto> Dependencies);