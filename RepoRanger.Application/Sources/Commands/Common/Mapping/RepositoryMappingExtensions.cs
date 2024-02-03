using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Commands.Common.Mapping;

internal static class RepositoryMappingExtensions
{
    public static IEnumerable<Repository> ToEntities(this IEnumerable<RepositoryDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }

    private static Repository ToEntity(this RepositoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var repository = new Repository(dto.Name, dto.RemoteUrl);
        repository.AddBranches(dto.Branches.ToEntities().ToList());

        return repository;
    }
}