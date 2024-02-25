using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.DependencyInstances.Common;

internal static class DependencyInstanceMappers
{
    public static IEnumerable<DependencyInstance> ToEntities(this IEnumerable<DependencyInstanceDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }

    private static DependencyInstance ToEntity(this DependencyInstanceDto instanceDto)
    {
        ArgumentNullException.ThrowIfNull(instanceDto);
        return new DependencyInstance(DependencySource.From(instanceDto.Source), instanceDto.Name, instanceDto.Version);
    }
    
    public static IEnumerable<DependencyInstanceDto> ToDtos(this IEnumerable<DependencyInstance> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        return dependencies.Select(ToDto);
    }

    private static DependencyInstanceDto ToDto(this DependencyInstance dependencyInstance)
    {
        ArgumentNullException.ThrowIfNull(dependencyInstance);
        return new DependencyInstanceDto(dependencyInstance.Source, dependencyInstance.DependencyName, dependencyInstance.Version);
    }
}