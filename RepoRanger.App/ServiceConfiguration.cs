using Quartz;
using RepoRanger.App.Jobs;
using RepoRanger.App.Middleware;
using RepoRanger.App.Services;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Persistence;
using Serilog;
using QuartzOptions = RepoRanger.App.Options.QuartzOptions;

namespace RepoRanger.App;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartzServices(configuration);
        services.AddExceptionHandlerServices();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        // services.AddSwaggerGen();
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
        services.Configure<QuartzOptions>(configuration.GetSection("QuartzOptions"));

        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();

            options.AddJob<RepoRangerJob>(RepoRangerJob.JobKey)
                .AddTrigger(trigger => trigger.ForJob(RepoRangerJob.JobKey).StartNow()
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(1).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }
}