namespace RepoRanger.Infrastructure.AzureDevOps.Models;

public sealed record AzureDevOpsProject(
    string Id,
    string Name,
    string Description,
    string Url,
    string State,
    int Revision,
    string Visibility,
    string LastUpdateTime
);