using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Domain.Common.Interfaces;

public interface ISourceFileParser
{
    bool CanParse(string filePath);
    Task<IEnumerable<Project>> ParseAsync(string content, FileInfo fileInfo, ParsingContext parsingContext);
}