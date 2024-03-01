using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Common.Interfaces;
using RepoRanger.Domain.Sources.Repositories.Git;
using RepoRanger.Infrastructure.Services;

namespace RepoRanger.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IGitDetailService, GitDetailService>();
        services.AddTransient<IDateTime, DateTimeService>();
    }
}