// ReSharper disable once CheckNamespace
namespace System.IO;

public static class GitDirectoryOperations
{
    public static IEnumerable<string> GetGitRepositoryPaths(this DirectoryInfo info)
    {
        var directory = info.FullName;
        return GetGitRepositoryPaths(directory);
    }

    private static IEnumerable<string> GetGitRepositoryPaths(string directory)
    {
        var repositoryPaths = new List<string>();
        foreach (var subdirectory in Directory.GetDirectories(directory))
        {
            if (Directory.Exists(Path.Combine(subdirectory, ".git")))
            {
                repositoryPaths.Add(subdirectory);
            }
            else
            {
                GetGitRepositoryPaths(subdirectory);
            }
        }

        return repositoryPaths;
    }
}