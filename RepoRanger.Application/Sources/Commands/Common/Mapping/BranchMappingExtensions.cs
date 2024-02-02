using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Commands.Common.Mapping;

internal static class BranchMappingExtensions
{
    public static IEnumerable<Branch> ToEntities(this IEnumerable<BranchDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);
        return dtos.Select(ToEntity);
    }

    private static Branch ToEntity(this BranchDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var branch = new Branch(dto.Name, dto.IsDefault);
        branch.AddProjects(dto.Projects.ToEntities());

        return branch;
    }
}