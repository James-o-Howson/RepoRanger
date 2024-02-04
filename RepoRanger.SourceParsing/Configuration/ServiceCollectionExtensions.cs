using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.SourceParsing.Services;

namespace RepoRanger.SourceParsing.Configuration;

internal static class ServiceCollectionExtensions
{
    public static void AddSourceParser(this IServiceCollection services, 
        IConfiguration configuration,
        Action<ISourceParserConfigurator> configure)
    {
        services.Configure<SourceParserOptions>(configuration.GetSection("SourceParsingOptions"));
        services.TryAddTransient<ISourceParser, SourceParserService>();
        
        var configurator = new SourceParserConfigurator(services, configuration);
        configure.Invoke(configurator);
    }
}