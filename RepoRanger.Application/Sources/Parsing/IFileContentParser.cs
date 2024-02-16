using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Parsing;

public interface IFileContentParser
{
    bool CanParse(string filePath);
    Task ParseAsync(string content, FileInfo fileInfo, Branch branch);
}