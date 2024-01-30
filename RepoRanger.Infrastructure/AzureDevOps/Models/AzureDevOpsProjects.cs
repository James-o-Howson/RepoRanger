namespace RepoRanger.Infrastructure.AzureDevOps.Models;

public sealed record AzureDevOpsProjects(
    int Count, 
    AzureDevOpsProject[] Value);