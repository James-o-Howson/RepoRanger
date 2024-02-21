using RepoRanger.Application.Branches;
using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoryDto(string Name, string RemoteUrl, BranchDto Branch, IEnumerable<ProjectDto> Projects);