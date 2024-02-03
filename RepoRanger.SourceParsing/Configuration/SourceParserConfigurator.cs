using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.SourceParsing.Services;

namespace RepoRanger.SourceParsing.Configuration;

internal interface ISourceParserConfigurator
{
    void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser;

    void AddSource(Action<ISourceBuilder> builder);
}

internal sealed class SourceParserConfigurator : ISourceParserConfigurator
{
    private readonly IServiceCollection _services;
    
    public SourceParserConfigurator(IServiceCollection services)
    {
        _services = services;
    }

    public void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser
    {
        _services.AddTransient<IFileContentParser, TFileContentParser>(provider => 
            ActivatorUtilities.CreateInstance<TFileContentParser>(provider));
    }
    
    public void AddSource(Action<ISourceBuilder> builder)
    {
        var sourceBuilder = new SourceBuilder();
        builder.Invoke(sourceBuilder);
        var options = sourceBuilder.Build();
        
        _services.Configure<SourceParserOptions>(s => s.Sources.Add(options));
    }
}