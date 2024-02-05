using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Sources.Parsing;

namespace RepoRanger.SourceParsing.Abstractions.Configuration;

internal interface ISourceParserConfigurator
{
    void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser;
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

    public void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IFileContentParser
    {
        _services.AddTransient<IFileContentParser, TFileContentParser>(provider => 
            ActivatorUtilities.CreateInstance<TFileContentParser>(provider));
    }
}