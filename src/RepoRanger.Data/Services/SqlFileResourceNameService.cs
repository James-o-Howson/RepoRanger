using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Data.Services;

internal sealed class SqlFileResourceNameService : IResourceNameService
{
    private const string? ResourceNamePrefix = "RepoRanger.Data.Sql";

    public string GetOrphanedDependenciesResourceName => $"{ResourceNamePrefix}.GetOrphanedDependencies.sql";
}