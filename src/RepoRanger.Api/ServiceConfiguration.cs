﻿using Quartz;
using RepoRanger.Api.Jobs;
using RepoRanger.Api.Middleware;
using RepoRanger.Api.Services;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Persistence;
using Serilog;
using QuartzOptions = RepoRanger.Api.Options.QuartzOptions;

namespace RepoRanger.Api;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddCors();
        services.AddQuartzServices(configuration, environment);
        services.AddExceptionHandlerServices();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        services.AddOpenApiDocument();
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
    
    public static void UseSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/RepoRanger_.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

        SerilogHostBuilderExtensions.UseSerilog(hostBuilder);
    }
    
    private static void AddExceptionHandlerServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
    
    private static void AddQuartzServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.Configure<QuartzOptions>(configuration.GetSection("QuartzOptions"));

        if (environment.IsIntegrationTest()) return;
        services.AddQuartz(configurator =>
        {
            configurator.UseSimpleTypeLoader();
            configurator.UseInMemoryStore();

            configurator.AddJob<RepoRangerJob>(RepoRangerJob.JobKey)
                .AddTrigger(trigger => trigger.ForJob(RepoRangerJob.JobKey).StartNow()
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(60).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }
}