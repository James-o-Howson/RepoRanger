using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;
using RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet;

internal sealed partial class DotNetProjectFileParser : IProjectFileParser
{
    [GeneratedRegex(Pattern, RegexOptions.Multiline)]
    private static partial Regex ProjectInfoRegex();
    
    private const string SolutionExtension = ".sln";
    private const string Pattern = """
                                   Project\("(.*?)"\)\s*=\s*"(.*?)"\s*,\s*"(.*?\.csproj)"\s*,\s*"(.*?)"
                                   """;

    private readonly IEnumerable<IProjectParser> _projectParsers;
    private readonly ILogger<DotNetProjectFileParser> _logger;

    public DotNetProjectFileParser(IEnumerable<IProjectParser> projectParsers, 
        ILogger<DotNetProjectFileParser> logger)
    {
        _projectParsers = projectParsers;
        _logger = logger;
    }

    public bool CanParse(string filePath) => 
        !string.IsNullOrEmpty(filePath) && 
        filePath.EndsWith(SolutionExtension);

    public async Task<IReadOnlyCollection<ProjectDescriptor>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo,
        ParsingContext parsingContext)
    {
        _logger.LogInformation("Parsing C# Solution {SolutionPath}", fileInfo.FullName);
        
        var content = await File.ReadAllTextAsync(fileInfo.FullName);
        var projectDefinitions = GetProjectDefinitions(fileInfo, content);
        var projects = await CreateProjects(parsingContext, fileInfo, projectDefinitions);

        _logger.LogInformation("Finished Parsing C# Solution {SolutionPath}. Projects found = {ProjectsCount}",
            fileInfo.FullName, projects.Count);

        return projects;
    }

    private async Task<IReadOnlyCollection<ProjectDescriptor>> CreateProjects(ParsingContext parsingContext, FileSystemInfo solutionFileInfo,
        IEnumerable<DotNetProjectDefinition> projectDefinitions)
    {
        var descriptors = new List<ProjectDescriptor>();
        foreach (var definition in projectDefinitions)
        {
            if (parsingContext.IsAlreadyParsed(definition.FilePath))
            {
                _logger.LogInformation("Skipping CSharp Project {CsprojFilePath}. Project has already been parsed", definition.FilePath);
                continue;
            }
            
            if (!definition.Exists)
            {
                parsingContext.MarkAsParsed(definition.FilePath, null);
                _logger.LogInformation("Skipping CSharp Project {CsprojFilePath}. Project in Solution File but not present in Repository", definition.FilePath);
                continue;
            }
            
            _logger.LogInformation("Parsing CSharp Project {CsprojFilePath}", definition.FilePath);

            var projectDependencyDescriptors = _projectParsers
                .SelectMany(p => p.ParseAsync(definition.Content)).ToHashSet();
            
            var projectDescriptor = new ProjectDescriptor(ProjectType.Dotnet, definition.Name, 
                await GetDotNetVersionAsync(definition.Content), 
                definition.FilePath, 
                GetMetadata(definition, solutionFileInfo), 
                projectDependencyDescriptors);
        
            descriptors.Add(projectDescriptor);
            
            _logger.LogInformation("Finished Parsing C# Project {CsprojFilePath}. Dependencies found = {DependencyCount}",
                definition.FilePath, projectDescriptor.ProjectDependencies.Count);
            
            parsingContext.MarkAsParsed(definition.FilePath, definition.FileInfo);
        }
        
        return descriptors;
    }

    private static IEnumerable<DotNetProjectDefinition> GetProjectDefinitions(FileInfo fileInfo, string content)
    {
        var regex = ProjectInfoRegex();
        var matches = regex.Matches(content);

        List<DotNetProjectDefinition> projectDefinitions = [];
        foreach (Match projectLine in matches)
        {
            var projectId = projectLine.Groups[4].Value;
            var projectTypeId = projectLine.Groups[1].Value;
            var projectFilePath = projectLine.Groups[3].Value;
            
            projectDefinitions.Add(new DotNetProjectDefinition
            {
                ProjectId = projectId,
                Type = projectTypeId,
                FilePath = Path.Join(Path.GetDirectoryName(fileInfo.FullName), projectFilePath)
            });
        }

        return projectDefinitions;
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

    private static List<ProjectMetadataDescriptor> GetMetadata(DotNetProjectDefinition definition, FileSystemInfo fileInfo) =>
    [
        new ProjectMetadataDescriptor("Solution Name", fileInfo.Name),
        new ProjectMetadataDescriptor("Visual Studio Project Guid", definition.ProjectId),
        new ProjectMetadataDescriptor("Visual Studio Type Guid", definition.Type)
    ];
}