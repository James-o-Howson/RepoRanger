using System.Xml.Linq;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.SourceParsing.DotNet.Projects;

internal sealed class ProjectPackageReferenceAttributeParser : IProjectParser
{
    public IEnumerable<DependencyInstance> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static IEnumerable<DependencyInstance> ParseProject(XContainer projectContainer)
    {
        var dependencyViewModels = projectContainer.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .Select(pr =>
            {
                var name = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var versionElement = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "Version");
                var version = versionElement?.Value.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(version))
                {
                    version = pr.Attribute("Version")?.Value.Trim() ?? string.Empty;
                }

                return new DependencyInstance(DependencySource.Nuget, name, version);
            });
        
        return dependencyViewModels;
    }
}