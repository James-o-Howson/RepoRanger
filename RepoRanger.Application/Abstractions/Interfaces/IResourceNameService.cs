namespace RepoRanger.Application.Abstractions.Interfaces;

public interface IResourceNameService
{
    string GetOrphanedDependencies { get; }
    string GetOrphanedProjects { get; }
}