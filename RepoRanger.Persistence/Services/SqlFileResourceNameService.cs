using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Persistence.Services;

internal sealed class SqlFileResourceNameService : IResourceNameService
{
    private const string? ResourceNamePrefix = "RepoRanger.Persistence.Sql";

    public string GetOrphanedDependencies => $"{ResourceNamePrefix}.GetOrphanedDependencies.sql";
    public string GetOrphanedProjects => $"{ResourceNamePrefix}.GetOrphanedProjects.sql";
}