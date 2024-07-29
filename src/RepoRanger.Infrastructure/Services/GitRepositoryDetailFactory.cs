using RepoRanger.Domain.VersionControlSystems.Git;

namespace RepoRanger.Infrastructure.Services;

internal sealed class GitRepositoryDetailFactory : IGitRepositoryDetailFactory
{
    public GitRepositoryDetail Create(DirectoryInfo info)
    {
        using var repo = new LibGit2Sharp.Repository(info.FullName);

        return new GitRepositoryDetail
        {
            Name = info.Name,
            BranchName = repo.Head.UpstreamBranchCanonicalName ?? repo.Head.CanonicalName,
            RemoteUrl = repo.Network.Remotes["origin"].Url
        };
    }
}