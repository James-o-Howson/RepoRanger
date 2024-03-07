// ReSharper disable once CheckNamespace
namespace System.IO;

public static class DirectoryInfoExtensions
{
    public static IEnumerable<string> GetGitRepositories(this DirectoryInfo info)
    {
        var directory = info.FullName;
        return GetGitRepositories(directory);
    }

    private static IEnumerable<string> GetGitRepositories(string directory)
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
                GetGitRepositories(subdirectory);
            }
        }

        return repositoryPaths;
    }
}