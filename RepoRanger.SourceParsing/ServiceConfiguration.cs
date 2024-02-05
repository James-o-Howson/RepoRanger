using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.SourceParsing.Abstractions.Configuration;
using RepoRanger.SourceParsing.Abstractions.Options;
using RepoRanger.SourceParsing.Angular;
using RepoRanger.SourceParsing.CSharp;

namespace RepoRanger.SourceParsing;

public static class ServiceConfiguration
{
    public static void AddSourceParsingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSourceParser(configuration, c =>
        {
            c.AddFileContentParser<CSharpProjectFileContentParser>();
            c.AddFileContentParser<AngularProjectFileContentParser>();
        });
    }
    
    private static void AddSourceParser(this IServiceCollection services, 
        IConfiguration configuration,
        Action<ISourceParserConfigurator> configure)
    {
        services.Configure<SourceParserOptions>(configuration.GetSection("SourceParserOptions"));
        services.TryAddTransient<ISourceParser, SourceParserService>();
        
        var configurator = new SourceParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}