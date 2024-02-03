using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing;

public interface IFileContentParser
{
    bool CanParse(FileInfo fileInfo);
    void Parse(string content, FileInfo fileInfo, BranchContext branchContext);
}