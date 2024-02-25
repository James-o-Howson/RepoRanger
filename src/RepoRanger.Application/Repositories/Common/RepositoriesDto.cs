namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoriesDto(IReadOnlyCollection<RepositoryDto> Repositories);