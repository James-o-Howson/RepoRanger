using RepoRanger.Application.DependencyInstances.Common;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.Projects.Common;

public static class ProjectMapper
{
    public static IEnumerable<Project> ToEntities(this IEnumerable<ProjectVm> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }
    
    private static Project ToEntity(this ProjectVm vm)
    {
        return Project.Create(
            ProjectType.From(vm.Type), 
            vm.Name, 
            vm.Version, 
            vm.Path,
            null, 
            vm.DependencyInstances.ToEntities());
    }
}