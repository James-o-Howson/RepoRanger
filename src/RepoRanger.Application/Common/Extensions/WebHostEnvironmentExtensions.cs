using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

public static class WebHostEnvironmentExtensions
{
    public static bool IsIntegrationTest(this IWebHostEnvironment environment) => 
        environment.EnvironmentName == "IntegrationTest";
}