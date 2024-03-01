using RepoRanger.Domain.Entities;

namespace RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

internal interface IProjectParser
{
    IEnumerable<DependencyInstance> ParseAsync(string projectContent);
}