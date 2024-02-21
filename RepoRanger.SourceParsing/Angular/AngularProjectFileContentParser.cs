using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Domain.Entities;

namespace RepoRanger.SourceParsing.Angular;

internal sealed class AngularProjectFileContentParser : IFileContentParser
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

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

    public async Task ParseAsync(string content, FileInfo fileInfo, Repository repository)
    {
        _logger.LogInformation("Parsing package.json {PackageJsonPath}", fileInfo.FullName);
        
        var package = await JsonSerializer.DeserializeAsync<PackageJson>(
            new MemoryStream(Encoding.UTF8.GetBytes(content)),
            JsonSerializerOptions);
        
        if (package is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(PackageJson)}", content);

        var project = new Project(package.Name, FindAngularVersion(package));
        project.AddDependencies(GetDependencies(package));
        repository.AddProject(project);
        
        _logger.LogInformation("Finished Parsing package.json {PackageJsonPath}. Dependencies found = {DependencyCount}", fileInfo.FullName, project.Dependencies.Count);
    }

    private static IEnumerable<Dependency> GetDependencies(PackageJson package)
    {
        var dependencies = package.DevDependencies
            .Select(entry => new Dependency(entry.Key, entry.Value))
            .ToList();
        dependencies.AddRange(package.Dependencies
            .Select(entry => new Dependency(entry.Key, entry.Value ?? string.Empty)));

        return dependencies;
    }

    private static string FindAngularVersion(PackageJson packageJson)
    {
        const string angularVersionKey = "@angular/core";
        _ = packageJson.Dependencies.TryGetValue(angularVersionKey, out var version);

        return version ?? string.Empty;
    }
}