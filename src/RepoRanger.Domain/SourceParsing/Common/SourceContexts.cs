namespace RepoRanger.Domain.SourceParsing.Common;

public sealed class SourceContexts
{
    public List<SourceContext> Sources { get; set; } = [];
}

public class SourceContext
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; }  = string.Empty;
    public bool Enabled { get; set; }
    public IEnumerable<string> ExcludedRepositories { get; set; } = new List<string>();

    public DirectoryInfo LocationInfo => new(Location);

    public bool IsExcluded(string repositoryPath)
    {
        var directoryName = Path.GetFileName(repositoryPath);
        if (string.IsNullOrEmpty(directoryName))
            throw new ArgumentException($"{nameof(repositoryPath)} is not a valid repository path");
        
        return ExcludedRepositories.Contains(directoryName);
    }
}