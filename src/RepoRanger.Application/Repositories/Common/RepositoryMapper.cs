using RepoRanger.Application.Projects.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Repositories.Common;

public static class RepositoryMapper
{
    public static IEnumerable<Repository> ToEntities(this IEnumerable<RepositoryDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }
    
    private static Repository ToEntity(this RepositoryDto dto)
    {
        return Repository.Create(
            dto.Name, 
            dto.RemoteUrl, 
            dto.Branch, 
            dto.Projects.ToEntities());
    }
}