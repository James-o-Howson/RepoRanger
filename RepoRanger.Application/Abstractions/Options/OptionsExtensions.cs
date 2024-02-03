using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Abstractions.Exceptions;

namespace RepoRanger.Application.Abstractions.Options;

public static class OptionsExtensions
{
    public static TOptions RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) 
        where TOptions : class, IOptions, new()
    {
        var options = TryLoad<TOptions>(configuration);
        services.Configure<TOptions>(configuration.GetSection(options.SectionName));
        
        return options;
    }
    
    private static TOptions TryLoad<TOptions>(this IConfiguration configuration) 
        where TOptions : IOptions, new()
    {
        var options = new TOptions();
        var sectionName = options.SectionName;

        options = configuration.GetSection(sectionName).Get<TOptions>();
        
        if (options is null) throw new NotFoundException($"Unable to find options of type {typeof(TOptions)} for section {sectionName}");
        if(!options.IsValid()) throw new ArgumentException($"Options invalid. {nameof(sectionName)}:{sectionName} for {typeof(TOptions)}");
            
        return options;
    }
}