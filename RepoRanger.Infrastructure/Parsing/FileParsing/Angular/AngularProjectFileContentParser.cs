using System.Text.Json;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Infrastructure.Parsing.FileParsing.Angular;

internal sealed class AngularProjectFileContentParser : IFileContentParser
{
    public bool CanParse(FileInfo fileInfo) => 
        fileInfo.Name == "package.json";

    public void Parse(string content, FileInfo fileInfo, BranchContext branchContext)
    {
        var package = JsonSerializer.Deserialize<PackageJson>(content);
        if (package is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(PackageJson)}", content);

        var projectContext = new ProjectContext
        {
            Name = package.Name,
            Version = FindAngularVersion(package)
        };
        
        projectContext.DependencyContexts.AddRange(GetDependencies(package));
        branchContext.ProjectContexts.Add(projectContext);
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