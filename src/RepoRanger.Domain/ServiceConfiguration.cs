using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddTransient<IRepositoryParser, RepositoryParser>();
        services.AddTransient<ISourceParser, SourceParser>();
    }
}