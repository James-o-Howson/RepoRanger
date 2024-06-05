using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Domain.SourceParsing.Common;

namespace RepoRanger.Domain.Common.Interfaces;

public interface ISourceFileParser
{
    bool CanParse(string filePath);
    Task<IEnumerable<Project>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo, ParsingContext parsingContext);
}