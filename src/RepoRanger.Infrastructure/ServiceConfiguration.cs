using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.VersionControlSystems.Git;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Infrastructure.Services;
using RepoRanger.Infrastructure.SourceParsing;
using RepoRanger.Infrastructure.SourceParsing.Angular;
using RepoRanger.Infrastructure.SourceParsing.Common;
using RepoRanger.Infrastructure.SourceParsing.DotNet;
using RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;
using ThirdPartyApiClient;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IGitRepositoryDetailFactory, GitRepositoryDetailFactory>();
        services.AddTransient<IDateTime, DateTimeService>();
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
        services.Configure<VersionControlSystemContexts>(configuration.GetSection("SourceParserOptions"));
        services.TryAddTransient<IVersionControlSystemParserService, VersionControlSystemParserService>();
        services.TryAddTransient<ISourceParserResultHandler, SourceParserResultHandler>();
        
        var configurator = new SourceParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}