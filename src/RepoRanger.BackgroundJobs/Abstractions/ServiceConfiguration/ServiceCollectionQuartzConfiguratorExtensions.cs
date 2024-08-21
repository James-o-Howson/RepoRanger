using RepoRanger.BackgroundJobs.Abstractions.ServiceConfiguration;

// ReSharper disable once CheckNamespace
namespace Quartz;

internal static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static ISequentialJobConfigurator AddSequentialJobs<TInitialJob>(this IServiceCollectionQuartzConfigurator configurator,
        JobKey initialJobKey, Action<ITriggerConfigurator> initialTriggerConfigurator)
        where TInitialJob : IJob
    {
        configurator.AddJob(typeof(TInitialJob), initialJobKey);
        configurator.AddTrigger(initialTriggerConfigurator);
        
        return new SequentialJobConfigurator(configurator);
    }
}