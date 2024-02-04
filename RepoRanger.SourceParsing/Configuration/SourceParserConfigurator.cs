using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.SourceParsing.Services;

namespace RepoRanger.SourceParsing.Configuration;

internal interface ISourceParserConfigurator
{
    void EnableSourcesViaAppSettings();
    void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser;

    void AddSource(Action<ISourceBuilder> builder);
}

internal sealed class SourceParserConfigurator : ISourceParserConfigurator
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public SourceParserConfigurator(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    public void EnableSourcesViaAppSettings()
    {
        _services.Configure<SourceParserOptions>(s => s.SourcesEnabledViaConfiguration = true);
    }

    public void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser
    {
        _services.AddTransient<IFileContentParser, TFileContentParser>(provider => 
            ActivatorUtilities.CreateInstance<TFileContentParser>(provider));
    }
    
    public void AddSource(Action<ISourceBuilder> builder)
    {
        var sourceBuilder = new SourceBuilder(_services);
        builder.Invoke(sourceBuilder);
        var options = sourceBuilder.Build();
        
        _services.Configure<SourceParserOptions>(s => s.Sources.Add(options));
    }
}