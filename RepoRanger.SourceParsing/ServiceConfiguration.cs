using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.SourceParsing.Angular;
using RepoRanger.SourceParsing.Configuration;
using RepoRanger.SourceParsing.CSharp;

namespace RepoRanger.SourceParsing;

public static class ServiceConfiguration
{
    public static void AddSourceParsingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSourceParser(configuration, c =>
        {
            c.AddFileContentParser<CSharpProjectFileContentParser>();
            c.AddFileContentParser<AngularProjectFileContentParser>();
            
            c.AddSource(source => source
                .WithName("AzureDevOps")
                .IsEnabled(true)
                .ExcludingRepositories(["ReportingFramework"])
                .WithWorkingDirectory(@"C:\Development\git"));
            
            c.AddSource(source => source
                .WithName("AzureDevOps")
                .IsEnabled(false)
                .WithWorkingDirectory(@"C:\Development\git")
                .ExcludingRepositories(["ReportingFramework"]));
        });
    }
}