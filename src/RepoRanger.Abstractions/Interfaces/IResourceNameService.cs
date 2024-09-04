namespace RepoRanger.Abstractions.Interfaces;

public interface IResourceNameService
{
    string GetOrphanedDependenciesResourceName { get; }
}