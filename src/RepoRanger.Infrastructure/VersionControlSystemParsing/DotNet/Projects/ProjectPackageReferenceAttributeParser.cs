using System.Xml.Linq;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects;

internal sealed class ProjectPackageReferenceAttributeParser : IProjectParser
{
    public IEnumerable<ProjectDependencyDescriptor> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static IEnumerable<ProjectDependencyDescriptor> ParseProject(XContainer projectContainer)
    {
        var dependencyViewModels = projectContainer.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .Select(pr =>
            {
                var dependencyName = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var versionElement = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "Version");
                var versionValue = versionElement?.Value.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(versionValue))
                {
                    versionValue = pr.Attribute("Version")?.Value.Trim() ?? string.Empty;
                }

                return new ProjectDependencyDescriptor(dependencyName, "Nuget", versionValue);
            });
        
        return dependencyViewModels;
    }
}