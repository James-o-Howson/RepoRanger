using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.VersionControlSystems.Parsing;

namespace RepoRanger.Infrastructure.VersionControlSystemParsing.Common;

internal interface ISourceParserConfigurator
{
    void AddFileContentParser<TFileContentParser>()
        where TFileContentParser : class, IProjectFileParser;
}

internal sealed class VcsParserConfigurator : ISourceParserConfigurator
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public VcsParserConfigurator(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    public void AddFileContentParser<TProjectExtractor>()
        where TProjectExtractor : class, IProjectFileParser
    {
        _services.AddTransient<IProjectFileParser, TProjectExtractor>(provider => 
            ActivatorUtilities.CreateInstance<TProjectExtractor>(provider));
    }
}