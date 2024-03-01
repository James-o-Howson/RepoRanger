namespace RepoRanger.Domain.Sources.Repositories.Git;

public interface IGitDetailService
{
    GitRepositoryDetail GetRepositoryDetail(DirectoryInfo info);
}