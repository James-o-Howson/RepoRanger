using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Common.Interfaces;
using RepoRanger.Domain.Git;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Infrastructure.Services;
using RepoRanger.Infrastructure.SourceParsing;
using RepoRanger.Infrastructure.SourceParsing.Angular;
using RepoRanger.Infrastructure.SourceParsing.Common;
using RepoRanger.Infrastructure.SourceParsing.DotNet;
using RepoRanger.Infrastructure.SourceParsing.DotNet.Projects;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IGitDetailService, GitDetailService>();
        services.AddTransient<IDateTime, DateTimeService>();
        
        services.AddTransient<IProjectParser, ProjectPackageReferenceAttributeParser>();
        services.AddTransient<IProjectParser, ProjectReferenceAttributeParser>();
        
        services.AddSourceParser(configuration, c =>
        {
            c.AddFileContentParser<DotNetSourceFileParser>();
            c.AddFileContentParser<AngularProjectSourceFileParser>();
        });
    }
    
    private static void AddSourceParser(this IServiceCollection services, 
        IConfiguration configuration,
        Action<ISourceParserConfigurator> configure)
    {
        services.Configure<SourceParserOptions>(configuration.GetSection("SourceParserOptions"));
        services.TryAddTransient<ISourceParserService, SourceParserService>();
        
        var configurator = new SourceParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}