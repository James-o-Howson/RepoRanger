using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.SourceParsing;

public sealed class ParsingContext
{
    private bool _isParsing;
    
    private ParsingContext() { }
    
    public IEnumerable<ISourceFileParser> SourceFileParsers { get; private init; } = Array.Empty<ISourceFileParser>();
    public int? SourceId { get; private set; }
    public DirectoryInfo? GitDirectory { get; set; }
    public string GitDirectoryPath => GitDirectory?.FullName ?? string.Empty;
    public string GitRepositoryName => GitDirectory?.Name ?? string.Empty;
    
    public void EnsureParsingContextValid()
    {
        if (_isParsing && GitDirectory != null && SourceId != null) return;

        throw new ArgumentException($"{nameof(ParsingContext)} is in an invalid state.");
    }

    public void StartParsing(int sourceId)
    {
        _isParsing = true;
        SourceId = sourceId;
    }

    public void StopParsing()
    {
        _isParsing = false;
        SourceId = null;
        GitDirectory = null;
    }

    public static ParsingContext Create(IEnumerable<ISourceFileParser> sourceFileParsers)
    {
        return new ParsingContext
        {
            SourceFileParsers = sourceFileParsers,
        };
    }
}