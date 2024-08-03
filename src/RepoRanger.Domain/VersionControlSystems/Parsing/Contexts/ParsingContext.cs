using System.Collections.Concurrent;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;

public sealed class ParsingContext
{
    private readonly ConcurrentDictionary<string, FileInfo> _parsedFiles = new();

    private ParsingContext() { }

    public ConcurrentQueue<IProjectFileParser> FileParsers { get; private init; } = new();

    public void MarkAsParsed(string path, FileInfo fileInfo)
    {
        _parsedFiles.TryAdd(Path.GetFullPath(path).ToLower(), fileInfo);
    }

    public bool IsAlreadyParsed(string path) => 
        _parsedFiles.ContainsKey(Path.GetFullPath(path).ToLower());

    public static ParsingContext Create(ConcurrentQueue<IProjectFileParser> fileParsers)
    {
        return new ParsingContext
        {
            FileParsers = fileParsers,
        };
    }
}