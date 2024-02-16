using RepoRanger.Application.Branches;

namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoryDto(string Name, string RemoteUrl, IEnumerable<BranchDto> Branches);