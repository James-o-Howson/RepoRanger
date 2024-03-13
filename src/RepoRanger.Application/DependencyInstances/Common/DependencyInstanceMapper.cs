using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.DependencyInstances.Common;

public static class DependencyInstanceMapper
{
    public static IEnumerable<DependencyInstance> ToEntities(this IEnumerable<DependencyInstanceVm> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }
    
    private static DependencyInstance ToEntity(this DependencyInstanceVm vm)
    {
        return DependencyInstance.Create(DependencySource.From(vm.Source), vm.Name, vm.Version);
    }
}