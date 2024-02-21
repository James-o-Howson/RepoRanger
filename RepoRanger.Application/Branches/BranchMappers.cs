using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Branches;

internal static class BranchMappers
{
    public static Branch ToEntity(this BranchDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new Branch(dto.Name, dto.IsDefault);
    }
    
    public static BranchDto ToDto(this Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        return new BranchDto(branch.Name, branch.IsDefault);
    }
}