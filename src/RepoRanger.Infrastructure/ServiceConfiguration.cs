using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.VersionControlSystems.Git;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Infrastructure.Services;
using RepoRanger.Infrastructure.VersionControlSystemParsing;
using RepoRanger.Infrastructure.VersionControlSystemParsing.Angular;
using RepoRanger.Infrastructure.VersionControlSystemParsing.Common;
using RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects;
using ThirdPartyApiClient;
using DotNetProjectFileParser = RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.DotNetProjectFileParser;
using ProjectReferenceAttributeParser = RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects.ProjectReferenceAttributeParser;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDependencyManagerFactory, DependencyManagerFactory>();
        services.AddTransient<IGitRepositoryDetailFactory, GitRepositoryDetailFactory>();
        services.AddTransient<IVulnerabilityService, VulnerabilitiesService>();
        
        services.AddTransient<IProjectParser, ProjectPackageReferenceAttributeParser>();
        services.AddTransient<IProjectParser, ProjectReferenceAttributeParser>();
        
        services.AddSourceParser(configuration, c =>
        {
            c.AddFileContentParser<DotNetProjectFileParser>();
            c.AddFileContentParser<AngularProjectProjectFileParser>();
        });

        services.AddHttpClient<IOpenSourceVulnerabilitiesClient, OpenSourceVulnerabilitiesClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.osv.dev/");
        }).AddStandardResilienceHandler();
    }
    
    private static void AddSourceParser(this IServiceCollection services, 
        IConfiguration configuration,
        Action<ISourceParserConfigurator> configure)
    {
        services.Configure<VersionControlSystemContexts>(configuration.GetSection("VersionControlSystemParserOptions"));
        services.TryAddTransient<IVersionControlSystemParserService, VcsParserService>();
        services.TryAddTransient<IVcsParserResultHandler, VcsParserResultHandler>();
        
        var configurator = new VcsParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}