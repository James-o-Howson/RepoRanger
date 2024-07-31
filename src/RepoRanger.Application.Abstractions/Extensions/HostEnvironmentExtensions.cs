// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

public static class HostEnvironmentExtensions
{
    public static bool IsIntegrationTest(this IHostEnvironment environment) => 
        environment.EnvironmentName == "IntegrationTest";
}