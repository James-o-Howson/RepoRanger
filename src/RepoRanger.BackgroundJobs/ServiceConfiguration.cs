using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using RepoRanger.BackgroundJobs.Jobs;

namespace RepoRanger.BackgroundJobs;

public static class ServiceConfiguration
{
    public static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<BackgroundJobOptions>(configuration.GetSection(BackgroundJobOptions.ConfigurationKey));

        services.AddVcsParserJob(environment);
    }

    private static void AddVcsParserJob(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsIntegrationTest()) return;
        services.AddQuartz(configurator =>
        {
            configurator.UseSimpleTypeLoader();
            configurator.UseInMemoryStore();

            configurator.AddJob<VcsParserJob>(VcsParserJob.JobKey)
                .AddTrigger(trigger => trigger.ForJob(VcsParserJob.JobKey).StartNow()
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(60).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }
}