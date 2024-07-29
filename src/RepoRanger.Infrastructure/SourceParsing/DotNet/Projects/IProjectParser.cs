using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

internal interface IProjectParser
{
    IEnumerable<ProjectDependencyDescriptor> ParseAsync(string projectContent);
}