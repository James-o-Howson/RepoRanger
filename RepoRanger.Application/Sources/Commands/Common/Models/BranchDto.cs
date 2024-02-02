namespace RepoRanger.Application.Sources.Commands.Common.Models;

public sealed record BranchDto(string Name, bool IsDefault, IEnumerable<ProjectDto> Projects);