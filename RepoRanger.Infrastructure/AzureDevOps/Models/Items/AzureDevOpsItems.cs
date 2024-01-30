namespace RepoRanger.Infrastructure.AzureDevOps.Models.Items;

public sealed record AzureDevOpsItems(
    int Count,
    AzureDevOpsItem[] Value
);