using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IRepositoryParser, RepositoryParser>();
        services.AddTransient<ISourceParser, SourceParser>();
    }
}