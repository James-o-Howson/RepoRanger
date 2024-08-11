namespace RepoRanger.Domain.VersionControlSystems;

public static class VersionControlSystemOperations
{
    public static IEnumerable<string> GetAllRepositoryNames(this IEnumerable<VersionControlSystem> collection) =>
        collection
            .SelectMany(v => v.Repositories)
            .Select(r => r.Name);
}