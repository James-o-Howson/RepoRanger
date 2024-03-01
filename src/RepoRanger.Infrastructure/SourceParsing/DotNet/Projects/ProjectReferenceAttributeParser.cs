using System.Text.RegularExpressions;
using System.Xml.Linq;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

internal sealed partial class ProjectReferenceAttributeParser : IProjectParser
{
    [GeneratedRegex(@"\d+(?:\.\d+)+")]
    private static partial Regex VersionRegex();
    
    public IEnumerable<DependencyInstance> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static IEnumerable<DependencyInstance> ParseProject(XContainer project)
    {
        var dependencyViewModels = project.Descendants()
            .Where(e => e.Name.LocalName == "Reference")
            .Select(pr =>
            {
                var include = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var parts = include.Split(",");

                var name = string.Empty;
                var version = string.Empty;
                
                switch (parts.Length)
                {
                    case > 1:
                        name = parts.First();
                        version = parts[1].Trim()["Version=".Length..];
                        break;
                    case 1:
                    {
                        name = parts.First();
                        var hintPath = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "HintPath");
                        if (hintPath is not null)
                        {
                            var matches = VersionRegex().Match(hintPath.Value.Trim());
                            version = matches.Value;
                        }

                        break;
                    }
                }
                    
                return DependencyInstance.CreateInstance(DependencySource.Local, name, version);
            });
        
        return dependencyViewModels;
    }
}