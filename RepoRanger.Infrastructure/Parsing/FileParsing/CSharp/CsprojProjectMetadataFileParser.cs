using System.Xml.Linq;

namespace RepoRanger.Infrastructure.Parsing.FileParsing.CSharp;

public interface ICsprojDotNetVersionFileParser
{
    string Parse(string content);
}

public sealed class CsprojDotNetVersionFileParser : ICsprojDotNetVersionFileParser
{
    public string Parse(string content) => GetDotNetVersion(content);

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