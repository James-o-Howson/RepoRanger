using RepoRanger.Application.Projects.Common;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Branches;

internal static class BranchMappers
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
    
    public static IEnumerable<BranchDto> ToDtos(this IEnumerable<Branch> branches)
    {
        ArgumentNullException.ThrowIfNull(branches);
        return branches.Select(ToDto);
    }

    private static BranchDto ToDto(this Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        return new BranchDto(branch.Name, branch.IsDefault, branch.Projects.ToDtos());
    }
}