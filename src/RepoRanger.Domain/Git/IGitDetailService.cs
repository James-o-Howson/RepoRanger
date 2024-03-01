namespace RepoRanger.Domain.Git;

public interface IGitDetailService
{
    GitRepositoryDetail GetRepositoryDetail(DirectoryInfo info);
}