using System.Collections.Concurrent;
using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.SourceParsing;

public sealed class ParsingContext : IDisposable
{
    private readonly ConcurrentDictionary<string, FileInfo?> _parsedFiles = new();

    private ParsingContext() { }

    public ConcurrentQueue<ISourceFileParser> SourceFileParsers { get; private init; } = new();
    public DirectoryInfo? GitDirectory { get; set; }
    public string GitDirectoryPath => GitDirectory?.FullName ?? string.Empty;
    public string GitRepositoryName => GitDirectory?.Name ?? string.Empty;

    public void MarkAsParsed(string path, FileInfo? fileInfo)
    {
        _parsedFiles.TryAdd(Path.GetFullPath(path).ToLower(), fileInfo);
    }

    public bool AlreadyParsed(string path) => 
        _parsedFiles.ContainsKey(Path.GetFullPath(path).ToLower());

    public static ParsingContext Create(ConcurrentQueue<ISourceFileParser> sourceFileParsers)
    {
        return new ParsingContext
        {
            SourceFileParsers = sourceFileParsers,
        };
    }

    public void Dispose() => 
        GitDirectory = null;
}