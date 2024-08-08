using RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects;

internal interface IProjectParser
{
    IEnumerable<ProjectDependencyDescriptor> ParseAsync(string projectContent);
}