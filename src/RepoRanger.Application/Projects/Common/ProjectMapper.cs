using RepoRanger.Application.DependencyInstances.Common;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.Projects.Common;

public static class ProjectMapper
{
    public static IEnumerable<Project> ToEntities(this IEnumerable<ProjectDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }
    
    private static Project ToEntity(this ProjectDto dto)
    {
        return Project.Create(
            ProjectType.From(dto.Type), 
            dto.Name, 
            dto.Version, 
            dto.Path,
            null, 
            dto.Dependencies.ToEntities());
    }
}