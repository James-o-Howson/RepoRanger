namespace RepoRanger.Application.Sources.Common.Models;

public record SourceDto(string Name, IEnumerable<RepositoryDto> Repositories);