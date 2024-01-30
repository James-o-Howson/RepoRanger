namespace RepoRanger.Infrastructure.AzureDevOps.Models;

public sealed record AzureDevOpsRepositories(
    AzureDevOpsRepository[] Value,
    int Count
);