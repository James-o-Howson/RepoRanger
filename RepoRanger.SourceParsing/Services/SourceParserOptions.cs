namespace RepoRanger.SourceParsing.Services;

internal sealed class SourceParserOptions
{
    public List<SourceOptions> Sources { get; set; } = [];
}

public class SourceOptions
{
    public string Name { get; set; } = string.Empty;
    public string SourceRepositoryParentDirectory { get; set; }  = string.Empty;
    public bool Enabled { get; set; }
    public IEnumerable<string> ExcludedRepositories { get; set; } = [];
}