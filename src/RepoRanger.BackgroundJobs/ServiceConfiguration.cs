using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using RepoRanger.BackgroundJobs.Abstractions.Options;
using RepoRanger.BackgroundJobs.Jobs;

namespace RepoRanger.BackgroundJobs;

public static class ServiceConfiguration
{
    public static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<BackgroundJobOptions>(configuration.GetSection(BackgroundJobOptions.ConfigurationKey));

        services.AddJobs(environment);
    }

    private static void AddJobs(this IServiceCollection services, IHostEnvironment environment)
    {
        if (environment.IsIntegrationTest()) return;
        services.AddQuartz(configurator =>
        {
            configurator.UseSimpleTypeLoader();
            configurator.UseInMemoryStore();
            
            configurator.AddJob<PersistedEventDispatcherJob>(PersistedEventDispatcherJob.JobKey);
            configurator.AddTrigger(trigger => trigger.SimpleTrigger(PersistedEventDispatcherJob.JobKey));
            
            configurator.AddSequentialJobs<VcsParserJob>(VcsParserJob.JobKey,
                    trigger => SimpleTrigger(trigger, VcsParserJob.JobKey))
                .ThenExecute<VulnerabilityDiscoveryJob>(VulnerabilityDiscoveryJob.JobKey);
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }

    private static void SimpleTrigger(this ITriggerConfigurator trigger, JobKey jobKey) =>
        trigger.ForJob(jobKey)
            .StartNow()
            .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(60).RepeatForever());
}