using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Abstractions.Events.Domain;

namespace RepoRanger.Abstractions;

public static class ServiceConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
    }
}