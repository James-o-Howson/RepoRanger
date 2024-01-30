namespace RepoRanger.Infrastructure.AzureDevOps.Models;

public sealed record AzureDevOpsItems(
    int Count,
    AzureDevOpsItem[] Value
);