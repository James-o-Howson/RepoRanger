﻿using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Domain.SourceParsing.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Infrastructure.SourceParsing.Angular;

internal sealed class AngularProjectSourceFileParser : ISourceFileParser
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly ILogger<AngularProjectSourceFileParser> _logger;

    public AngularProjectSourceFileParser(ILogger<AngularProjectSourceFileParser> logger)
    {
        _logger = logger;
    }

    public bool CanParse(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        if (!filePath.EndsWith("package.json")) return false;

        var directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directory)) return false;

        var workspaceIsSibling = Path.Exists(Path.Combine(directory, "angular.json"));
        return workspaceIsSibling;
    }

    public async Task<IEnumerable<Project>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo, ParsingContext parsingContext)
    {
        ArgumentNullException.ThrowIfNull(gitRepository);
        _logger.LogInformation("Parsing package.json {PackageJsonPath}", fileInfo.FullName);

        if (parsingContext.IsAlreadyParsed(fileInfo.FullName))
        {
            _logger.LogInformation("Skipping Angular Project {CsprojFilePath}. Project has already been parsed", fileInfo.FullName);
            return [];
        }
        
        var content = await File.ReadAllTextAsync(fileInfo.FullName);
        var package = await JsonSerializer.DeserializeAsync<PackageJson>(
            new MemoryStream(Encoding.UTF8.GetBytes(content)),
            JsonSerializerOptions);
        
        if (package is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(PackageJson)}", content);

        var relativePath = fileInfo.RelativeTo(gitRepository);
        var project = Project.Create(ProjectType.Angular, package.Name, FindAngularVersion(package), relativePath, null);
        project.AddDependencyInstances(GetDependencyInstances(package));
        
        _logger.LogInformation("Finished Parsing package.json {PackageJsonPath}. Dependencies found = {DependencyCount}", fileInfo.FullName, project.DependencyInstances.Count);
        parsingContext.MarkAsParsed(fileInfo.FullName, fileInfo);
        
        return [project];
    }

    private static IEnumerable<DependencyInstance> GetDependencyInstances(PackageJson package)
    {
        var dependencies = package.DevDependencies
            .Select(entry => CreateDependencyInstance(entry.Key, entry.Value))
            .ToList();
        
        dependencies.AddRange(package.Dependencies
            .Select(entry => CreateDependencyInstance(entry.Key, entry.Value)));
        
        return dependencies.ToHashSet();
    }

    private static string FindAngularVersion(PackageJson packageJson)
    {
        const string angularVersionKey = "@angular/core";
        _ = packageJson.Dependencies.TryGetValue(angularVersionKey, out var version);

        return version ?? string.Empty;
    }

    private static DependencyInstance CreateDependencyInstance(string dependencyName, string? version)
    {
        if (dependencyName.Length > 0 && dependencyName[0] == '@')
        {
            dependencyName = dependencyName[1..];
        }
        
        return DependencyInstance.Create(DependencySource.Npm, dependencyName, version ?? string.Empty);
    }
}