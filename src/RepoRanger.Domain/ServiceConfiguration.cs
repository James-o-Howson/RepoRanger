using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Factories;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Updaters;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddTransient<IVersionControlSystemParser, VersionControlSystemParser>();
        services.AddTransient<IVersionControlSystemUpdater, VersionControlSystemUpdater>();
        services.AddTransient<IVersionControlSystemFactory, VersionControlSystemFactory>();
        
        services.AddTransient<IRepositoryParser, RepositoryParser>();
        services.AddTransient<IRepositoryUpdater, RepositoryUpdater>();
        services.AddTransient<IRepositoryFactory, RepositoryFactory>();
        
        services.AddTransient<IProjectUpdater, ProjectUpdater>();
        services.AddTransient<IProjectFactory, ProjectFactory>();
        services.AddTransient<IProjectMetadataFactory, ProjectMetadataFactory>();
        services.AddTransient<IProjectDependencyFactory, ProjectDependencyFactory>();

        services.AddTransient<IDependencyManager, DependencyManager>();
    }
}