
using RepoRanger.Application.Branches;
using RepoRanger.Application.Projects.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Repositories.Common;

internal static class RepositoryMappers
{
    public static IEnumerable<Repository> ToEntities(this IEnumerable<RepositoryDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }

    private static Repository ToEntity(this RepositoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var repository = new Repository(dto.Name, dto.RemoteUrl, dto.Branch.ToEntity());
        repository.AddProjects(dto.Projects.ToEntities().ToList());

        return repository;
    }
    
    public static IEnumerable<RepositoryDto> ToDtos(this IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        return repositories.Select(ToDto);
    }

    private static RepositoryDto ToDto(this Repository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        return new RepositoryDto(repository.Name, repository.RemoteUrl, repository.DefaultBranch.ToDto(), repository.Projects.ToDtos());
    }
}