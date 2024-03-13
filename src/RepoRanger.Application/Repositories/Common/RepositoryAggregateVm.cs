using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoryAggregateVm(int Id, string Name, string RemoteUrl, string Branch, IEnumerable<ProjectVm> Projects);