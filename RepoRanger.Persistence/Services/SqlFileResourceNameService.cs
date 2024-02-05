using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Persistence.Services;

internal sealed class SqlFileResourceNameService : IResourceNameService
{
    private static readonly string? ResourceNamePrefix = typeof(SqlFileResourceNameService).Namespace;

    public string GetOrphanedDependencies { get; } = $"{ResourceNamePrefix}.GetOrphanedDependencies.sql";
    public string GetOrphanedProjects { get; } = $"{ResourceNamePrefix}.GetOrphanedProjects.sql";
}