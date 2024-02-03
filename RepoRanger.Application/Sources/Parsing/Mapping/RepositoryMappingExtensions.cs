using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing.Mapping;

public static class RepositoryMappingExtensions
{
    public static IEnumerable<RepositoryDto> ToDtos(this IEnumerable<RepositoryContext> contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        return contexts.Select(ToDto);
    }

    private static RepositoryDto ToDto(this RepositoryContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new RepositoryDto(context.Name, context.RemoteUrl, context.BranchContexts.ToDtos());
    }
}