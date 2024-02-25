using RepoRanger.Application.DependencyInstances.Common;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.Projects.Common;

internal static class ProjectMappers
{
    public static IEnumerable<Project> ToEntities(this IEnumerable<ProjectDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        
        return dtos.Select(ToEntity);
    }

    private static Project ToEntity(this ProjectDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var project = new Project(ProjectType.From(dto.Type), dto.Name, dto.Version);
        project.AddDependencies(dto.Dependencies.ToEntities());

        return project;
    }
    
    public static IEnumerable<ProjectDto> ToDtos(this IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        return projects.Select(ToDto);
    }

    private static ProjectDto ToDto(this Project project)
    {
        ArgumentNullException.ThrowIfNull(project);
        return new ProjectDto(project.Type, project.Name, project.Version, project.DependencyInstances.ToDtos());
    }
}