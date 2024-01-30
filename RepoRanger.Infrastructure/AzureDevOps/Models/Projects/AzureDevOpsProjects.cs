namespace RepoRanger.Infrastructure.AzureDevOps.Models.Projects;

public sealed record AzureDevOpsProjects(
    int Count, 
    AzureDevOpsProject[] Value);