using RepoRanger.Domain.Sources.Repositories.Git;

namespace RepoRanger.SourceParsing.Services;

internal sealed class GitDetailService : IGitDetailService
{
    public GitRepositoryDetail GetRepositoryDetail(DirectoryInfo info)
    {
        using var repo = new LibGit2Sharp.Repository(info.FullName);

        return new GitRepositoryDetail
        {
            Name = repo.Head.UpstreamBranchCanonicalName ?? repo.Head.CanonicalName,
            BranchName = info.Name,
            RemoteUrl = repo.Network.Remotes["origin"].Url
        };
    }
}