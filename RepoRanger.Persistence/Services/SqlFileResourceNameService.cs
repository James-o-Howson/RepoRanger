using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Persistence.Services;

internal sealed class SqlFileResourceNameService : IResourceNameService
{
    private const string? ResourceNamePrefix = "RepoRanger.Persistence.Sql";

    public string GetOrphanedDependenciesResourceName => $"{ResourceNamePrefix}.GetOrphanedDependencies.sql";
    public string GetOrphanedProjectsResourceName => $"{ResourceNamePrefix}.GetOrphanedProjects.sql";
}