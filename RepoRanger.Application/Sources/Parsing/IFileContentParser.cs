using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing;

public interface IFileContentParser
{
    bool CanParse(string filePath);
    Task ParseAsync(string content, FileInfo fileInfo, BranchContext branchContext);
}