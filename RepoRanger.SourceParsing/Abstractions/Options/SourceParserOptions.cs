namespace RepoRanger.SourceParsing.Abstractions.Options;

internal sealed class SourceParserOptions
{
    public List<SourceOptions> Sources { get; set; } = [];
}

public class SourceOptions
{
    public string Name { get; set; } = string.Empty;
    public string WorkingDirectory { get; set; }  = string.Empty;
    public bool Enabled { get; set; }
    public IEnumerable<string> ExcludedRepositories { get; set; } = new List<string>();

    public bool IsExcluded(string repositoryPath)
    {
        var directoryName = Path.GetFileName(repositoryPath);
        if (string.IsNullOrEmpty(directoryName))
            throw new ArgumentException($"{nameof(repositoryPath)} is not a valid repository path");
        
        return ExcludedRepositories.Contains(directoryName);
    }
}