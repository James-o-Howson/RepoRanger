using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.SourceParsing;

public sealed class ParsingContext : IDisposable
{
    private ParsingContext() { }
    
    public IEnumerable<ISourceFileParser> SourceFileParsers { get; private init; } = Array.Empty<ISourceFileParser>();
    public DirectoryInfo? GitDirectory { get; set; }
    public string GitDirectoryPath => GitDirectory?.FullName ?? string.Empty;
    public string GitRepositoryName => GitDirectory?.Name ?? string.Empty;
    
    public static ParsingContext Create(IEnumerable<ISourceFileParser> sourceFileParsers)
    {
        return new ParsingContext
        {
            SourceFileParsers = sourceFileParsers,
        };
    }

    public void Dispose() => 
        GitDirectory = null;
}