using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Domain.Common;
using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.SourceParsing.Common.Configuration;

internal interface ISourceParserConfigurator
{
    void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, ISourceFileParser;
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

    public void AddFileContentParser<TProjectExtractor>()
        where TProjectExtractor : class, ISourceFileParser
    {
        _services.AddTransient<ISourceFileParser, TProjectExtractor>(provider => 
            ActivatorUtilities.CreateInstance<TProjectExtractor>(provider));
    }
}