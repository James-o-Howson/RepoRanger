using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.SourceParsing.Angular;
using RepoRanger.SourceParsing.Common.Configuration;
using RepoRanger.SourceParsing.Common.Options;
using RepoRanger.SourceParsing.DotNet;
using RepoRanger.SourceParsing.DotNet.Projects;
using RepoRanger.SourceParsing.Services;
using ProjectReferenceAttributeParser = RepoRanger.SourceParsing.DotNet.Projects.ProjectReferenceAttributeParser;

namespace RepoRanger.SourceParsing;

public static class ServiceConfiguration
{
    public static void AddSourceParsingServices(this IServiceCollection services, IConfiguration configuration)
    {
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
        services.TryAddTransient<IGitParserService, GitParserService>();
        
        var configurator = new SourceParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}