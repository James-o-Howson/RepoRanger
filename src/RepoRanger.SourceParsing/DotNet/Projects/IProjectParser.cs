using RepoRanger.Domain.Entities;

namespace RepoRanger.SourceParsing.DotNet.Projects;

internal interface IProjectParser
{
    IEnumerable<DependencyInstance> ParseAsync(string projectContent);
}