using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RepoRanger.Application.Abstractions.Options;

public static class OptionsExtensions
{
    public static TOptions TryLoad<TOptions>(this IConfiguration configuration) 
        where TOptions : IOptions, new()
    {
        return TryLoad<TOptions>(configuration, null);
    }
    
    public static TOptions TryLoad<TOptions>(this IConfiguration configuration, IHostEnvironment environment) 
        where TOptions : IOptions, new()
    {
        var options = new TOptions();
        options = configuration.GetSection(options.SectionName).Get<TOptions>();
        
        if (options != null && !options.IsValid(environment)) 
            throw new ArgumentException($@"Options invalid. {nameof(options.SectionName)}:{options.SectionName} for {typeof(TOptions)}", nameof(options));
        
        return options;
    }

    public static TOptions RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) 
        where TOptions : class, IOptions, new()
    {
        var options = TryLoad<TOptions>(configuration);
        services.Configure<TOptions>(configuration.GetSection(options.SectionName));
        
        return options;
    }
}