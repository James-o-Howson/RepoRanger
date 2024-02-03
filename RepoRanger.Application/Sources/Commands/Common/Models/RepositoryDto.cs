namespace RepoRanger.Application.Sources.Commands.Common.Models;

public sealed record RepositoryDto(string Name, string RemoteUrl, IEnumerable<BranchDto> Branches);