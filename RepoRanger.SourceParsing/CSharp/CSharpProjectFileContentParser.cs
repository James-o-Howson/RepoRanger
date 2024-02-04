using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.SourceParsing.CSharp;

internal sealed class CSharpProjectFileContentParser : IFileContentParser
{
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

    public async Task ParseAsync(string content, FileInfo fileInfo, BranchContext branchContext)
    {
        _logger.LogInformation("Parsing CSharp Project {CsprojFilePath}", fileInfo.FullName);
        
        var projectContext = new ProjectContext
        {
            Name = fileInfo.Name,
            Version = await GetDotNetVersionAsync(content)
        };

        var dependencyContexts = GetDependencyContexts(content);
        projectContext.DependencyContexts.AddRange(dependencyContexts);
        branchContext.ProjectContexts.Add(projectContext);
        
        _logger.LogInformation("Finished Parsing CSharp Project {CsprojFilePath}. Dependencies found = {DependencyCount}", fileInfo.FullName, projectContext.DependencyContexts.Count);
    }

    private static IEnumerable<DependencyContext> GetDependencyContexts(string content)
    {
        var doc = XDocument.Parse(content);

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

                return new DependencyContext
                {
                    Name = name,
                    Version = version
                };
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

        return string.Join(", ", versions);
    }

    private static string? GetElementValue(XContainer document, string elementName)
    {
        var element = document.Descendants().FirstOrDefault(e => e.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase));
        return element?.Value;
    }
}