using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoryDto(string Name, string RemoteUrl, string Branch, IEnumerable<ProjectDto> Projects);