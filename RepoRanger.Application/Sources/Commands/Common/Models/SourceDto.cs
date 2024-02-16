namespace RepoRanger.Application.Sources.Commands.Common.Models;

public record SourceDto(string Name, IEnumerable<RepositoryDto> Repositories);