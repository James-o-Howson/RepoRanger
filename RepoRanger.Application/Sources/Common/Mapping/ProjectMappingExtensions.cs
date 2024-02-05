using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Common.Mapping;

internal static class ProjectMappingExtensions
{
    public static IEnumerable<Project> ToEntities(this IEnumerable<ProjectDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        
        return dtos.Select(ToEntity);
    }

    private static Project ToEntity(this ProjectDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var project = new Project(dto.Name, dto.Version);
        project.AddDependencies(dto.Dependencies.ToEntities());

        return project;
    }
}