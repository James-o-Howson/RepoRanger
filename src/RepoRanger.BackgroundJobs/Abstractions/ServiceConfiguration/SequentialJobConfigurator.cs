using Quartz;

namespace RepoRanger.BackgroundJobs.Abstractions.ServiceConfiguration;

internal interface ISequentialJobConfigurator
{
    SequentialJobConfigurator ThenExecute<TJob>(JobKey jobKey)
        where TJob : IJob;
}

internal sealed class SequentialJobConfigurator : ISequentialJobConfigurator
{
    private readonly IServiceCollectionQuartzConfigurator _configurator;

    public SequentialJobConfigurator(IServiceCollectionQuartzConfigurator configurator)
    {
        _configurator = configurator;
    }

    public SequentialJobConfigurator ThenExecute<TJob>(JobKey jobKey)
        where TJob : IJob
    {
        _configurator.AddJob(typeof(TJob), jobKey, 
            configurator => configurator.StoreDurably());
        return this;
    }
}