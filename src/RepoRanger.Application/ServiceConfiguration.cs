using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Common.Behaviours;
using RepoRanger.Application.Events;

namespace RepoRanger.Application;

public static class ServiceConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatrServices();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<ITransientEventDispatcher, TransientEventDispatcher>();
        services.AddScoped<IDurableEventDispatcher, DurableEventDispatcher>();
    }
    
    private static void AddMediatrServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
    }
}