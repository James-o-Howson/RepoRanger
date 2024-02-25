using RepoRanger.Application.Common.Interfaces;

namespace RepoRanger.Persistence.Services;

internal sealed class SqlFileResourceNameService : IResourceNameService
{
    private const string? ResourceNamePrefix = "RepoRanger.Persistence.Sql";

    public string GetOrphanedDependenciesResourceName => $"{ResourceNamePrefix}.GetOrphanedDependencies.sql";
}