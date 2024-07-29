namespace RepoRanger.Domain.VersionControlSystems.Git;

public interface IGitRepositoryDetailFactory
{
    GitRepositoryDetail Create(DirectoryInfo info);
}