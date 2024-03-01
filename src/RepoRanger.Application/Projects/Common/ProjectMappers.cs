using RepoRanger.Application.DependencyInstances.Common;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.Projects.Common;

internal static class ProjectMappers
{
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