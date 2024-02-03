using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Infrastructure.AzureDevOps;
using RepoRanger.Infrastructure.Services;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        
        services.AddAzureDevOpsService(configuration);
    }

    private static void AddAzureDevOpsService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureDevOpsOptions>(configuration.GetSection("AzureDevOpsOptions"));
        
        services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<AzureDevOpsOptions>>().Value;
            
            client.BaseAddress = options.BaseAddressUri();
            client.DefaultRequestHeaders.Authorization = options.AuthenticationHeader();

        }).AddStandardResilienceHandler();
    }
}