using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing.Angular;

internal sealed class AngularProjectProjectFileParser : IProjectFileParser
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly ILogger<AngularProjectProjectFileParser> _logger;

    public AngularProjectProjectFileParser(ILogger<AngularProjectProjectFileParser> logger)
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

    public async Task<IReadOnlyCollection<ProjectDescriptor>> ParseAsync(DirectoryInfo gitRepository, FileInfo fileInfo, ParsingContext parsingContext)
    {
        ArgumentNullException.ThrowIfNull(gitRepository);
        _logger.LogInformation("Parsing package.json {PackageJsonPath}", fileInfo.FullName);

        if (parsingContext.IsAlreadyParsed(fileInfo.FullName))
        {
            _logger.LogInformation("Skipping Angular Project {CsprojFilePath}. Project has already been parsed", fileInfo.FullName);
            return [];
        }
        
        await using var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
        var content = await fileStream.ReadAsync();
        
        var package = await JsonSerializer.DeserializeAsync<PackageJson>(
            new MemoryStream(Encoding.UTF8.GetBytes(content)),
            JsonSerializerOptions);
        
        if (package is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(PackageJson)}", content);

        var relativePath = fileInfo.RelativeTo(gitRepository);
        var projectDescriptor = new ProjectDescriptor(ProjectType.Angular, package.Name, FindAngularVersion(package), relativePath, [], GetProjectDependencies(package));
        
        _logger.LogInformation("Finished Parsing package.json {PackageJsonPath}. Dependencies found = {DependencyCount}", fileInfo.FullName, projectDescriptor.ProjectDependencies.Count);
        parsingContext.MarkAsParsed(fileInfo.FullName, fileInfo);
        
        return [projectDescriptor];
    }

    private static HashSet<ProjectDependencyDescriptor> GetProjectDependencies(PackageJson package)
    {
        var dependencies = package.DevDependencies
            .Select(entry => CreateDependencyDescriptor(entry.Key, entry.Value))
            .ToList();
        
        dependencies.AddRange(package.Dependencies
            .Select(entry => CreateDependencyDescriptor(entry.Key, entry.Value)));
        
        return dependencies.ToHashSet();
    }

    private static string? FindAngularVersion(PackageJson packageJson)
    {
        const string angularVersionKey = "@angular/core";
        _ = packageJson.Dependencies.TryGetValue(angularVersionKey, out var version);

        return version;
    }

    private static ProjectDependencyDescriptor CreateDependencyDescriptor(string dependencyName, string? version)
    {
        if (dependencyName.Length > 0 && dependencyName[0] == '@')
        {
            dependencyName = dependencyName[1..];
        }

        return new ProjectDependencyDescriptor(dependencyName, "npm", version);
    }
}