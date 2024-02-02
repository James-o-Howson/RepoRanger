using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Commands.Common.Models;

namespace RepoRanger.Infrastructure.FileParsing.CSharp;

public interface ICsprojDependencyFileParser
{
    bool CanParse(string filePath);
    IEnumerable<DependencyDto> Parse(string content);
}

public sealed class CsprojDependencyFileParser : ICsprojDependencyFileParser
{
    private readonly ILogger<CsprojDependencyFileParser> _logger;

    public CsprojDependencyFileParser(ILogger<CsprojDependencyFileParser> logger)
    {
        _logger = logger;
    }

    public bool CanParse(string filePath) => filePath.EndsWith(".csproj");

    public IEnumerable<DependencyDto> Parse(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        
        var doc = XDocument.Parse(content);
        var dependencyViewModels = doc.XPathSelectElements("//PackageReference")
            .Select(pr =>
            {
                var name = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var version = pr.Attribute("Version")?.Value.Trim() ?? string.Empty;
                
                return new DependencyDto(name, version);
            }).ToList();

        _logger.LogInformation("Project file contains {DependencyCount} dependency references:", dependencyViewModels.Count);
        foreach (var packageReference in dependencyViewModels)
        {
            _logger.LogInformation("{DependencyName}, version {DependencyVersion}", packageReference.Name, packageReference.Version);
        }

        return dependencyViewModels;
    }
}