namespace RepoRanger.Application.Sources.Queries.GetSourceDetails;

public sealed class SourceDetailVm
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public DateTime ParseTime { get; init; }
    public Guid DefaultBranchId { get; init; }
    public string? DefaultBranchName { get; init; }
    public long ProjectsCount { get; init; }
    public long DependenciesCount { get; init; }
}