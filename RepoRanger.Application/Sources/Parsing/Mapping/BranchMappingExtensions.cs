using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing.Mapping;

public static class BranchMappingExtensions
{
    public static IEnumerable<BranchDto> ToDtos(this IEnumerable<BranchContext> contexts)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        return contexts.Select(ToDto);
    }

    private static BranchDto ToDto(this BranchContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new BranchDto(context.Name, context.IsDefault, context.ProjectContexts.ToDtos());
    }
}