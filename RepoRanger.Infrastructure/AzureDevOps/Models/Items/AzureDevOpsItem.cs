namespace RepoRanger.Infrastructure.AzureDevOps.Models.Items;

public sealed record AzureDevOpsItem(
    string ObjectId,
    string GitObjectType,
    string CommitId,
    string Path,
    bool IsFolder,
    string Url
);