using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.SourceParsing.Angular;

internal sealed class AngularProjectFileContentParser : IFileContentParser
{
    private readonly ILogger<AngularProjectFileContentParser> _logger;

    public AngularProjectFileContentParser(ILogger<AngularProjectFileContentParser> logger)
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

    public async Task ParseAsync(string content, FileInfo fileInfo, BranchContext branchContext)
    {
        _logger.LogInformation("Parsing package.json {PackageJsonPath}", fileInfo.FullName);
        
        var package = await JsonSerializer.DeserializeAsync<PackageJson>(
            new MemoryStream(Encoding.UTF8.GetBytes(content)),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (package is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(PackageJson)}", content);

        var projectContext = new ProjectContext
        {
            Name = package.Name,
            Version = FindAngularVersion(package)
        };
        
        projectContext.DependencyContexts.AddRange(GetDependencies(package));
        branchContext.ProjectContexts.Add(projectContext);
        
        _logger.LogInformation("Finished Parsing package.json {PackageJsonPath}. Dependencies found = {DependencyCount}", fileInfo.FullName, projectContext.DependencyContexts.Count);
    }

    private static IEnumerable<DependencyContext> GetDependencies(PackageJson package)
    {
        var dependencies = package.DevDependencies.Select(kvp => new DependencyContext
        {
            Name = kvp.Key,
            Version = kvp.Value
        }).ToList();
        
        dependencies.AddRange(package.Dependencies.Select(kvp => new DependencyContext
        {
            Name = kvp.Key,
            Version = kvp.Value ?? string.Empty
        }));

        return dependencies;
    }

    private static string FindAngularVersion(PackageJson packageJson)
    {
        const string angularVersionKey = "@angular/core";
        _ = packageJson.Dependencies.TryGetValue(angularVersionKey, out var version);

        return version ?? string.Empty;
    }
}