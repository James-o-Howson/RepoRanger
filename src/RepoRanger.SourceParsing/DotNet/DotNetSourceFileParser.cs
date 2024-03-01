using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;
using RepoRanger.SourceParsing.DotNet.Projects;

namespace RepoRanger.SourceParsing.DotNet;

internal sealed partial class DotNetSourceFileParser : ISourceFileParser
{
    [GeneratedRegex(Pattern, RegexOptions.Multiline)]
    private static partial Regex ProjectInfoRegex();
    
    private const string SolutionExtension = ".sln";
    private const string Pattern = """
                                   Project\("(.*?)"\)\s*=\s*"(.*?)"\s*,\s*"(.*?\.csproj)"\s*,\s*"(.*?)"
                                   """;

    private readonly IEnumerable<IProjectParser> _projectParsers;
    private readonly ILogger<DotNetSourceFileParser> _logger;

    public DotNetSourceFileParser(IEnumerable<IProjectParser> projectParsers, 
        ILogger<DotNetSourceFileParser> logger)
    {
        _projectParsers = projectParsers;
        _logger = logger;
    }

    public bool CanParse(string filePath) => 
        !string.IsNullOrEmpty(filePath) && 
        filePath.EndsWith(SolutionExtension);

    public async Task<IEnumerable<Project>> ParseAsync(string content, FileInfo fileInfo)
    {
        _logger.LogInformation("Parsing C# Solution {SolutionPath}", fileInfo.FullName);
        
        var projectDefinitions = GetProjectDefinitions(fileInfo, content);
        var projects = await CreateProjects(projectDefinitions, fileInfo);
        
        _logger.LogInformation("Finished Parsing C# Solution {SolutionPath}. Projects found = {ProjectsCount}",
            fileInfo.FullName, projects.Count);

        return projects;
    }

    private async Task<List<Project>> CreateProjects(IEnumerable<ProjectDefinition> projectDefinitions, FileSystemInfo fileInfo)
    {
        var projects = new List<Project>();
        foreach (var definition in projectDefinitions)
        {
            _logger.LogInformation("Parsing CSharp Project {CsprojFilePath}", definition.FilePath);
            
            var dependencyInstances = _projectParsers
                .SelectMany(p => p.ParseAsync(definition.Content));
            
            var project = Project.CreateNew(ProjectType.Dotnet, definition.Name, 
                await GetDotNetVersionAsync(definition.Content), GetMetadata(definition, fileInfo));
        
            project.AddDependencies(dependencyInstances);
            projects.Add(project);
            
            _logger.LogInformation("Finished Parsing C# Project {CsprojFilePath}. Dependencies found = {DependencyCount}",
                definition.FilePath, project.DependencyInstances.Count);
        }

        return projects;
    }

    private static IEnumerable<ProjectDefinition> GetProjectDefinitions(FileInfo fileInfo, string content)
    {
        var regex = ProjectInfoRegex();
        var matches = regex.Matches(content);

        List<ProjectDefinition> projects = [];
        foreach (Match projectLine in matches)
        {
            var projectId = projectLine.Groups[4].Value;
            var projectTypeId = projectLine.Groups[1].Value;
            var projectFilePath = projectLine.Groups[3].Value;
            
            projects.Add(new ProjectDefinition
            {
                ProjectId = projectId,
                Type = projectTypeId,
                FilePath = Path.Join(Path.GetDirectoryName(fileInfo.FullName), projectFilePath)
            });
        }

        return projects;
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

    private static Metadata[] GetMetadata(ProjectDefinition definition, FileSystemInfo fileInfo)
    {
        return [
            Metadata.Create("Solution Name", fileInfo.Name), 
            Metadata.Create("Visual Studio Project Guid", definition.ProjectId), 
            Metadata.Create("Visual Studio Type Guid", definition.Type) 
        ];
    }
}