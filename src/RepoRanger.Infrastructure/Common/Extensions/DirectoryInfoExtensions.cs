// ReSharper disable once CheckNamespace
namespace System.IO;

internal static class DirectoryInfoExtensions
{
    public static IEnumerable<string> GetGitDirectories(this DirectoryInfo info)
    {
        var directory = info.FullName;
        return GetGitDirectories(directory);
    }

    private static IEnumerable<string> GetGitDirectories(string directory)
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
                GetGitDirectories(subdirectory);
            }
        }

        return repositoryPaths;
    }
}