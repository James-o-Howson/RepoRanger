using Quartz;
using RepoRanger.Api.Jobs;
using RepoRanger.Application.Options;

namespace RepoRanger.Api.Configuration.Quartz;

internal static class QuartzStartupExtensions
{
    public static void AddQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<QuartzSettings>(configuration);

        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();

            options.ScheduleJob<RepositoryDataCollectorJob>(trigger =>
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