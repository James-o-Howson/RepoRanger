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
            c.EnableSourcesViaAppSettings();
            c.AddFileContentParser<CSharpProjectFileContentParser>();
            c.AddFileContentParser<AngularProjectFileContentParser>();
            
            c.AddSource(source => source
                .WithName("AzureDevOps")
                .ExcludingRepositories(["ReportingFramework", "Toms.Net Second"])
                .WithWorkingDirectory(@"C:\Development\git"));
            
            c.AddSource(source => source
                .WithName("Gitolite")
                .WithWorkingDirectory(@"C:\Development\git")
                .ExcludingRepositories(["ReportingFramework"]));
        });
    }
}