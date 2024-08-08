using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Synchronizers;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddTransient(typeof(ICollectionSynchronizer<,>), typeof(CollectionSynchronizer<,>));
        
        // VCS
        services.AddTransient<IVersionControlSystemParser, VersionControlSystemParser>();
        services.AddTransient<IVersionControlSystemSynchronizer, VersionControlSystemSynchronizer>();
        services.AddTransient<IVersionControlSystemFactory, VersionControlSystemFactory>();
        
        // Repository
        services.AddTransient<IRepositoryParser, RepositoryParser>();
        services.AddTransient<IRepositoryUpdater, RepositorySynchronizer>();
        services.AddTransient<IRepositoryFactory, RepositoryFactory>();
        
        // Project
        services.AddTransient<IProjectUpdater, ProjectSynchronizer>();
        services.AddTransient<IProjectFactory, ProjectFactory>();
        services.AddTransient<IProjectMetadataFactory, ProjectMetadataFactory>();
        services.AddTransient<IProjectDependencyFactory, ProjectDependencyFactory>();

        // Dependency
        services.AddTransient<IDependencyManager, DependencyManager>();
    }
}