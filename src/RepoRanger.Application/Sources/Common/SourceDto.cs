using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Sources.Common;

public record SourceDto(string Name, string Location, IEnumerable<RepositoryDto> Repositories);