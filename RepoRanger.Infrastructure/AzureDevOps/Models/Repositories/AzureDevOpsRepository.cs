using RepoRanger.Infrastructure.AzureDevOps.Models.Projects;

namespace RepoRanger.Infrastructure.AzureDevOps.Models.Repositories;

public sealed record AzureDevOpsRepository(
    string Id,
    string Name,
    string Url,
    AzureDevOpsProject AzureDevOpsProject,
    string DefaultBranch,
    int Size,
    string RemoteUrl,
    string SshUrl,
    string WebUrl,
    bool IsDisabled,
    bool IsInMaintenance
);