using FluentValidation;
using MediatR;
using RepoRanger.Abstractions.Behaviours;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Api.Infrastructure;
using RepoRanger.Api.Services;
using RepoRanger.Commands;
using RepoRanger.Data;
using RepoRanger.EventHandlers;
using RepoRanger.Queries;
using Serilog;

namespace RepoRanger.Api;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddCors();
        services.AddExceptionHandlerServices();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        services.AddOpenApiDocument(settings =>
        {
            settings.Title = "Repo Ranger API";
            settings.Version = "v1";
            settings.Description = "API for managing repositories in Repo Ranger.";
        });
        services.AddHttpContextAccessor();
        services.AddMediatr();
        services.AddValidatorsFromAssemblies([CommandsAssembly.Assembly, QueriesAssembly.Assembly]);

        services.AddScoped<IUser, CurrentUser>();
        services.AddSingleton(TimeProvider.System);
    }
    
    public static void AddLogging(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/RepoRanger_.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
    
    private static void AddExceptionHandlerServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(CommandsAssembly.Assembly,
                QueriesAssembly.Assembly, EventHandlersAssembly.Assembly);
            
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
    }
}