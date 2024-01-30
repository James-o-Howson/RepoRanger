namespace RepoRanger.Infrastructure.AzureDevOps.Models.Repositories;

public sealed record AzureDevOpsRepositories(
    AzureDevOpsRepository[] Value,
    int Count
);