using System.Collections.Concurrent;
using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.SourceParsing;

public sealed class ParsingContext
{
    private readonly ConcurrentDictionary<string, FileInfo?> _parsedFiles = new();

    private ParsingContext() { }

    public ConcurrentQueue<ISourceFileParser> SourceFileParsers { get; private init; } = new();
    // public DirectoryInfo? GitRepository { get; set; }
    // public string GitRepositoryPath => GitRepository?.FullName ?? string.Empty;
    // public string GitRepositoryName => GitRepository?.Name ?? string.Empty;

    public void MarkAsParsed(string path, FileInfo? fileInfo)
    {
        _parsedFiles.TryAdd(Path.GetFullPath(path).ToLower(), fileInfo);
    }

    public bool IsAlreadyParsed(string path) => 
        _parsedFiles.ContainsKey(Path.GetFullPath(path).ToLower());

    public static ParsingContext Create(ConcurrentQueue<ISourceFileParser> sourceFileParsers)
    {
        return new ParsingContext
        {
            SourceFileParsers = sourceFileParsers,
        };
    }
}