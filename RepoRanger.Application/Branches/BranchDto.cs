using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Branches;

public sealed record BranchDto(string Name, bool IsDefault, IEnumerable<ProjectDto> Projects);