using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.DependencyInstances.Common;

public static class DependencyInstanceMapper
{
    public static IEnumerable<DependencyInstance> ToEntities(this IEnumerable<DependencyInstanceDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }
    
    private static DependencyInstance ToEntity(this DependencyInstanceDto dto)
    {
        return DependencyInstance.Create(DependencySource.From(dto.Source), dto.Name, dto.Version);
    }
}