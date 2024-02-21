namespace RepoRanger.Application.Common.Interfaces;

public interface IResourceNameService
{
    string GetOrphanedDependenciesResourceName { get; }
}