﻿using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Domain.SourceParsing.Common;
using RepoRanger.Domain.ValueObjects;
using RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

namespace RepoRanger.Infrastructure.SourceParsing.DotNet;

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

    public async Task<IEnumerable<Project>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo,
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

    private async Task<List<Project>> CreateProjects(ParsingContext parsingContext, FileSystemInfo solutionFileInfo,
        IEnumerable<ProjectDefinition> projectDefinitions)
    {
        var projects = new List<Project>();
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

            var dependencyInstances = _projectParsers
                .SelectMany(p => p.ParseAsync(definition.Content)).ToHashSet();
            
            var project = Project.Create(ProjectType.Dotnet, definition.Name, 
                await GetDotNetVersionAsync(definition.Content), definition.FilePath, GetMetadata(definition, solutionFileInfo));
        
            project.AddDependencyInstances(dependencyInstances);
            projects.Add(project);
            
            _logger.LogInformation("Finished Parsing C# Project {CsprojFilePath}. Dependencies found = {DependencyCount}",
                definition.FilePath, project.DependencyInstances.Count);
            
            parsingContext.MarkAsParsed(definition.FilePath, definition.FileInfo);
        }
        
        return projects;
    }

    private static IEnumerable<ProjectDefinition> GetProjectDefinitions(FileInfo fileInfo, string content)
    {
        var regex = ProjectInfoRegex();
        var matches = regex.Matches(content);

        List<ProjectDefinition> projectDefinitions = [];
        foreach (Match projectLine in matches)
        {
            var projectId = projectLine.Groups[4].Value;
            var projectTypeId = projectLine.Groups[1].Value;
            var projectFilePath = projectLine.Groups[3].Value;
            
            projectDefinitions.Add(new ProjectDefinition
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

    private static IReadOnlyCollection<Metadata> GetMetadata(ProjectDefinition definition, FileSystemInfo fileInfo)
    {
        return [
            Metadata.Create("Solution Name", fileInfo.Name), 
            Metadata.Create("Visual Studio Project Guid", definition.ProjectId), 
            Metadata.Create("Visual Studio Type Guid", definition.Type) 
        ];
    }
}