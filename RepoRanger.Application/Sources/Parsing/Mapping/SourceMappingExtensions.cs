using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing.Mapping;

public static class SourceMappingExtensions
{
    public static IEnumerable<SourceDto> ToDtos(this IEnumerable<SourceContext> contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        return contexts.Select(ToDto);
    }
    
    public static SourceDto ToDto(this SourceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new SourceDto(context.Name, context.RepositoryContexts.ToDtos());
    }
}