using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing.Mapping;

public static class DependencyMappingExtensions
{
    public static IEnumerable<DependencyDto> ToDtos(this IEnumerable<DependencyContext> contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        return contexts.Select(ToDto);
    }

    private static DependencyDto ToDto(this DependencyContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new DependencyDto(context.Name, context.Version);
    }
}