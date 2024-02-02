using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Common.Mapping;

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
        
        var repository = new Repository(dto.Name, dto.Url, dto.RemoteUrl);
        repository.AddBranches(dto.Branches.ToEntities().ToList());

        return repository;
    }
}