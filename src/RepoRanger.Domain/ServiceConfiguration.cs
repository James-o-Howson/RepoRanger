using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Sources.Repositories;

namespace RepoRanger.Domain;

public static class ServiceConfiguration
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IGitRepositoryParser, GitRepositoryParser>();
    }
}