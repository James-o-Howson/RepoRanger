namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;

public sealed class VersionControlSystemContexts
{
    public const string SectionKey = "VersionControlSystemParserOptions";
    public required List<VersionControlSystemContext> Values { get; init; } = [];
}

public class VersionControlSystemContext
{
    public required string Name { get; init; }
    public required string Location { get; init; }
    public required bool Enabled { get; init; }
    public required IEnumerable<string> ExcludedRepositories { get; init; } = [];

    public DirectoryInfo LocationInfo => new(Location);

    public bool IsExcluded(string repositoryPath)
    {
        var directoryName = Path.GetFileName(repositoryPath);
        if (string.IsNullOrEmpty(directoryName))
            throw new ArgumentException($"{nameof(repositoryPath)} is not a valid repository path");
        
        return ExcludedRepositories.Contains(directoryName);
    }
}