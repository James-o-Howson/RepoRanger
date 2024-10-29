using Quartz;

namespace RepoRanger.BackgroundJobs.Abstractions.Options;

internal sealed class BackgroundJobOptions
{
    internal const string ConfigurationKey = "BackgroundJobOptions";
    public required List<JobOptions> Jobs { get; init; } = [];

    public bool IsEnabled(JobKey jobKey) =>
        Jobs.SingleOrDefault(job => job.JobName == jobKey.Name)?.Enabled ?? false;
    
    public JobOptions GetOptions(JobKey jobKey) =>
        Jobs.Single(job => job.JobName == jobKey.Name);
    
    public JobKey? NextJobKey(JobKey currentJobKey)
    {
        var jobOptions = Jobs.Single(job => Equals(job.JobKey, currentJobKey));

        return jobOptions.NextJobName is null ? 
            null : 
            new JobKey(jobOptions.NextJobName);
    }
}