using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.DependencyInstances.Common;

internal static class DependencyInstanceMappers
{
    public static IEnumerable<DependencyInstanceVm> ToDtos(this IEnumerable<DependencyInstance> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        return dependencies.Select(ToDto);
    }

    private static DependencyInstanceVm ToDto(this DependencyInstance dependencyInstance)
    {
        ArgumentNullException.ThrowIfNull(dependencyInstance);
        return new DependencyInstanceVm(dependencyInstance.Id, dependencyInstance.Source, dependencyInstance.DependencyName, dependencyInstance.Version);
    }
}