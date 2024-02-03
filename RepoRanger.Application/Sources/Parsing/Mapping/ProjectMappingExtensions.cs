using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing.Mapping;

public static class ProjectMappingExtensions
{
    public static IEnumerable<ProjectDto> ToDtos(this IEnumerable<ProjectContext> contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        return contexts.Select(ToDto);
    }

    private static ProjectDto ToDto(this ProjectContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new ProjectDto(context.Name, context.Version, context.DependencyContexts.ToDtos());
    }
}