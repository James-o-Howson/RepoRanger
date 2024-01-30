using RepoRanger.Application.Options;
using RepoRanger.Infrastructure.AzureDevOps;

namespace RepoRanger.Api.Configuration.AzureDevOps;

internal static class AzureDevOpsStartupExtensions
{
    public static void AddAzureDevOpsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var options = services.RegisterOptions<AzureDevOpsSettings>(configuration);
        
        services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>(client =>
        {
            client.BaseAddress = options.BaseAddressUri();
            client.DefaultRequestHeaders.Authorization = options.AuthenticationHeader();

        }).AddStandardResilienceHandler();
    }
}