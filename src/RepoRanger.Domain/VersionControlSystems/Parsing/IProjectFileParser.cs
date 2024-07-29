using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public interface IProjectFileParser
{
    bool CanParse(string filePath);
    Task<IReadOnlyCollection<ProjectDescriptor>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo, ParsingContext parsingContext);
}