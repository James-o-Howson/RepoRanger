using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.DependencyInstances.Common;

internal static class DependencyInstanceMappers
{
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