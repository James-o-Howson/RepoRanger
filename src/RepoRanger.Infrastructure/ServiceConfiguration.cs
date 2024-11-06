using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.VersionControlSystems.Git;
using RepoRanger.Domain.VersionControlSystems.Parsing;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Infrastructure.Events;
using RepoRanger.Infrastructure.OutboxMessages;
using RepoRanger.Infrastructure.VersionControlSystemParsing;
using RepoRanger.Infrastructure.VersionControlSystemParsing.Angular;
using RepoRanger.Infrastructure.VersionControlSystemParsing.Common;
using RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects;
using RepoRanger.Infrastructure.Vulnerabilities;
using RepoRanger.ThirdPartyClients.Generated;
using DotNetProjectFileParser = RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.DotNetProjectFileParser;
using ProjectReferenceAttributeParser = RepoRanger.Infrastructure.VersionControlSystemParsing.DotNet.Projects.ProjectReferenceAttributeParser;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIntegrationEventPublisher, IntegrationEventPublisher>();
        services.AddTransient<IDependencyManagerFactory, DependencyManagerFactory>();
        services.AddTransient<IGitRepositoryDetailFactory, GitRepositoryDetailFactory>();
        services.AddTransient<IExternalVulnerabilityService, ExternalVulnerabilitiesService>();
        services.AddTransient<IOutboxMessageDispatcher, OutboxMessageDispatcher>();
        services.Configure<OutboxProcessingOptions>(configuration.GetSection(OutboxProcessingOptions.SectionKey));
        
        services.AddTransient<IProjectParser, ProjectPackageReferenceAttributeParser>();
        services.AddTransient<IProjectParser, ProjectReferenceAttributeParser>();
        
        services.AddSourceParser(configuration, c =>
        {
            c.AddFileContentParser<DotNetProjectFileParser>();
            c.AddFileContentParser<AngularProjectProjectFileParser>();
        });
        
        services.Configure<OsvClientOptions>(configuration.GetSection(OsvClientOptions.SectionKey));
        services.AddHttpClient<IOsvClient, OsvClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<OsvClientOptions>>().Value;
            client.BaseAddress = options.BaseUrlUri;
        }).AddStandardResilienceHandler();
    }
    
    private static void AddSourceParser(this IServiceCollection services, 
        IConfiguration configuration,
        Action<ISourceParserConfigurator> configure)
    {
        services.Configure<VersionControlSystemContexts>(configuration.GetSection(VersionControlSystemContexts.SectionKey));
        services.TryAddTransient<IVersionControlSystemParserService, VcsParserService>();
        
        var configurator = new VcsParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}