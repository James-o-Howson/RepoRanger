using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Commands.Common.Mapping;

internal static class DependencyMappingExtensions
{
    public static IEnumerable<Dependency> ToEntities(this IEnumerable<DependencyDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }

    private static Dependency ToEntity(this DependencyDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new Dependency(dto.Name, dto.Version);
    }
    
    public static IEnumerable<DependencyDto> ToDtos(this IEnumerable<Dependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        return dependencies.Select(ToDto);
    }

    private static DependencyDto ToDto(this Dependency dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return new DependencyDto(dependency.Name, dependency.Version);
    }
}