using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Synchronizers;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Factories;
using RepoRanger.Domain.VersionControlSystems.Synchronizers.Updaters;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomain(this IServiceCollection services)
    {
        // VCS
        services.AddTransient<IVersionControlSystemSynchronizer, VersionControlSystemSynchronizer>();
        services.AddTransient<IVersionControlSystemParser, VersionControlSystemParser>();
        services.AddTransient<IVersionControlSystemUpdater, VersionControlSystemUpdater>();
        services.AddTransient<IVersionControlSystemFactory, VersionControlSystemFactory>();
        
        // Repository
        services.AddTransient<IRepositoryParser, RepositoryParser>();
        services.AddTransient<IRepositoryUpdater, RepositoryUpdater>();
        services.AddTransient<IRepositoryFactory, RepositoryFactory>();
        
        // Project
        services.AddTransient<IProjectUpdater, ProjectUpdater>();
        services.AddTransient<IProjectFactory, ProjectFactory>();
        services.AddTransient<IProjectMetadataFactory, ProjectMetadataFactory>();
        services.AddTransient<IProjectDependencyFactory, ProjectDependencyFactory>();

        // Dependency
        services.AddTransient<IDependencyManager, DependencyManager>();
    }
}