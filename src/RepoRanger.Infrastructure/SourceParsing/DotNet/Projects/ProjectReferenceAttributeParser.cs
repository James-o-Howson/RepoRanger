using System.Text.RegularExpressions;
using System.Xml.Linq;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

internal sealed partial class ProjectReferenceAttributeParser : IProjectParser
{
    [GeneratedRegex(@"\d+(?:\.\d+)+")]
    private static partial Regex VersionRegex();
    
    public IEnumerable<ProjectDependencyDescriptor> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static IEnumerable<ProjectDependencyDescriptor> ParseProject(XContainer project)
    {
        var dependencyViewModels = project.Descendants()
            .Where(e => e.Name.LocalName == "Reference")
            .Select(pr =>
            {
                var include = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var parts = include.Split(",");

                var dependencyName = string.Empty;
                var versionValue = string.Empty;
                
                switch (parts.Length)
                {
                    case > 1:
                        dependencyName = parts.First();
                        versionValue = parts[1].Trim()["Version=".Length..];
                        break;
                    case 1:
                    {
                        dependencyName = parts.First();
                        var hintPath = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "HintPath");
                        if (hintPath is not null)
                        {
                            var matches = VersionRegex().Match(hintPath.Value.Trim());
                            versionValue = matches.Value;
                        }

                        break;
                    }
                }

                return new ProjectDependencyDescriptor(dependencyName, "Local", versionValue);
            });
        
        return dependencyViewModels;
    }
}