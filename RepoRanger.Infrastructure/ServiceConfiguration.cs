using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Options;
using RepoRanger.Infrastructure.AzureDevOps;
using RepoRanger.Infrastructure.FileParsing.CSharp;
using RepoRanger.Infrastructure.Services;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsprojDependencyFileParser, CsprojDependencyFileParser>();
        services.AddTransient<ICsprojDotNetVersionFileParser, CsprojDotNetVersionFileParser>();
        
        services.AddAzureDevOpsService(configuration);
    }

    private static void AddAzureDevOpsService(this IServiceCollection services, IConfiguration configuration)
    {
        var options = services.RegisterOptions<AzureDevOpsOptions>(configuration);
        
        services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>(client =>
        {
            client.BaseAddress = options.BaseAddressUri();
            client.DefaultRequestHeaders.Authorization = options.AuthenticationHeader();

        }).AddStandardResilienceHandler();

        services.AddScoped<IAzureDevOpsRepositoryDataExtractor, AzureDevOpsRepositoryDataExtractor>();
    }
}