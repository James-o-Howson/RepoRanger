using RepoRanger.Domain.Git;

namespace RepoRanger.Infrastructure.Services;

internal sealed class GitDetailService : IGitDetailService
{
    public GitRepositoryDetail GetRepositoryDetail(DirectoryInfo info)
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