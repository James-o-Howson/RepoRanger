using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Domain.Entities;

namespace RepoRanger.SourceParsing.CSharp;

internal sealed partial class CSharpProjectFileContentParser : IFileContentParser
{
    [GeneratedRegex(@"\d+(?:\.\d+)+")]
    private static partial Regex VersionRegex();
    
    private readonly ILogger<CSharpProjectFileContentParser> _logger;

    public CSharpProjectFileContentParser(ILogger<CSharpProjectFileContentParser> logger)
    {
        _logger = logger;
    }
    
    public bool CanParse(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        return filePath.EndsWith(".csproj");
    }

    public async Task ParseAsync(string content, FileInfo fileInfo, Repository repository)
    {
        _logger.LogInformation("Parsing CSharp Project {CsprojFilePath}", fileInfo.FullName);
        
        var project = new Project(fileInfo.Name, await GetDotNetVersionAsync(content));

        var dependencies = GetDependencyContexts(content);
        project.AddDependencies(dependencies);
        
        repository.AddProject(project);
        
        _logger.LogInformation("Finished Parsing CSharp Project {CsprojFilePath}. Dependencies found = {DependencyCount}", fileInfo.FullName, project.Dependencies.Count);
    }

    private static IEnumerable<Dependency> GetDependencyContexts(string content)
    {
        var doc = XDocument.Parse(content);

        var dependencyContexts = GetDependenciesFromPackageReferenceAttribute(doc).ToList();
        dependencyContexts.AddRange(GetDependenciesFromReferenceAttribute(doc));
        dependencyContexts.RemoveAll(d => string.IsNullOrEmpty(d.Version));
        
        return dependencyContexts;
    }
    
    private static IEnumerable<Dependency> GetDependenciesFromReferenceAttribute(XContainer doc)
    {
        var dependencyViewModels = doc.Descendants()
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
                    
                return new Dependency(name, version);
            });
        
        return dependencyViewModels;
    }

    private static IEnumerable<Dependency> GetDependenciesFromPackageReferenceAttribute(XContainer doc)
    {
        var dependencyViewModels = doc.Descendants()
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

                return new Dependency(name, version);
            });
        return dependencyViewModels;
    }

    private static async Task<string> GetDotNetVersionAsync(string csprojContent)
    {
        var document = await XDocument.LoadAsync(new StringReader(csprojContent), LoadOptions.None, CancellationToken.None);

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
            
            versions.Add(value.Trim());
        }

        var version = string.Join(", ", versions);
        
        return $"Dotnet {version}";
    }

    private static string? GetElementValue(XContainer document, string elementName)
    {
        var element = document.Descendants().FirstOrDefault(e => e.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase));
        return element?.Value;
    }
}