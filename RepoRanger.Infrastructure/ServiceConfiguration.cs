using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Options;
using RepoRanger.Application.Sources.Parsing;
using RepoRanger.Infrastructure.AzureDevOps;
using RepoRanger.Infrastructure.Parsing;
using RepoRanger.Infrastructure.Parsing.FileParsing.Angular;
using RepoRanger.Infrastructure.Parsing.FileParsing.CSharp;
using RepoRanger.Infrastructure.Services;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        
        services.AddSourceParsingServices(configuration);
        services.AddAzureDevOpsService(configuration);
    }

    private static void AddSourceParsingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IFileContentParser, CSharpProjectFileContentParser>();
        services.AddTransient<IFileContentParser, AngularProjectFileContentParser>();

        services.AddTransient<ISourceParser, SourceParser>();
        services.RegisterOptions<SourceParserOptions>(configuration);
    }

    private static void AddAzureDevOpsService(this IServiceCollection services, IConfiguration configuration)
    {
        var options = services.RegisterOptions<AzureDevOpsOptions>(configuration);
        
        services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>(client =>
        {
            client.BaseAddress = options.BaseAddressUri();
            client.DefaultRequestHeaders.Authorization = options.AuthenticationHeader();

        }).AddStandardResilienceHandler();
    }
}