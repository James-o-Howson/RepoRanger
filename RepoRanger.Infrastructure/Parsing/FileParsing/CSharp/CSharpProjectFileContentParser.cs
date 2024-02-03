using System.Xml.Linq;
using System.Xml.XPath;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Infrastructure.Parsing.FileParsing.CSharp;

internal sealed class CSharpProjectFileContentParser : IFileContentParser
{
    public bool CanParse(FileInfo fileInfo) => 
        fileInfo.Extension == ".csproj";

    public void Parse(string content, FileInfo fileInfo, BranchContext branchContext)
    {
        var projectContext = new ProjectContext
        {
            Name = fileInfo.Name,
            Version = GetDotNetVersion(content)
        };

        var dependencyContexts = GetDependencyContexts(content);
        projectContext.DependencyContexts.AddRange(dependencyContexts);
    }
    
    private static IEnumerable<DependencyContext> GetDependencyContexts(string content)
    {
        var doc = XDocument.Parse(content);
        var dependencyViewModels = doc.XPathSelectElements("//PackageReference")
            .Select(pr =>
            {
                var name = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var version = pr.Attribute("Version")?.Value.Trim() ?? string.Empty;
                
                return new DependencyContext
                {
                    Name = name,
                    Version = version
                };
            });

        return dependencyViewModels;
    }
    
    private static string GetDotNetVersion(string csprojContent)
    {
        var document = XDocument.Load(new StringReader(csprojContent));

        string[] versionElements =
        [
            "TargetFramework",
            "TargetFrameworks",
            "TargetFrameworkIdentifier",
            "TargetFrameworkVersion",
            "TargetFrameworkMoniker",
            "TargetFrameworkProfile",
            "TargetFrameworkRootNamespace",
            "RuntimeIdentifier"
        ];

        var versions = new List<string>();
        foreach (var element in versionElements)
        {
            var value = GetElementValue(document, element);
            if (string.IsNullOrEmpty(value)) continue;
            
            versions.Add($"{element}:{value.Trim()}");
        }

        return string.Join(", ", versions);
    }

    private static string? GetElementValue(XContainer document, string elementName)
    {
        var element = document.Descendants().FirstOrDefault(e => e.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase));
        return element?.Value;
    }
}