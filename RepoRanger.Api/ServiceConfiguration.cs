using Quartz;
using RepoRanger.Api.Jobs;
using RepoRanger.Api.Middleware;
using RepoRanger.Api.Services;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Options;
using RepoRanger.Persistence;
using Serilog;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartzServices(configuration);
        services.AddExceptionHandlerServices();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
    
    public static void UseSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        SerilogHostBuilderExtensions.UseSerilog(hostBuilder);
    }
    
    private static void AddExceptionHandlerServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
    
    private static void AddQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<QuartzOptions>(configuration);

        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();

            options.ScheduleJob<RepoRangerJob>(trigger =>
            {
                const string description = "Trigger scheduled every 5 minutes beginning on API startup";
                trigger.WithIdentity("10 Minute Scheduled Trigger")
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(b => b.WithIntervalInSeconds(1))
                    .WithDescription(description);
            });
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }
}