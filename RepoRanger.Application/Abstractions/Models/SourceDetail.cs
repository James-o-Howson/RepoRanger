namespace RepoRanger.Application.Abstractions.Models;

public class SourceDetail
{
    private SourceDetail() { }
    
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime ParseTime { get; set; }
    public Guid DefaultBranchId { get; set; }
    public string? DefaultBranchName { get; set; }
    public long ProjectsCount { get; set; }
    public long DependenciesCount { get; set; }
}