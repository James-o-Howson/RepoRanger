namespace RepoRanger.Application.Sources.Common.Models;

public sealed record BranchDto(string Name, bool IsDefault, IEnumerable<ProjectDto> Projects);