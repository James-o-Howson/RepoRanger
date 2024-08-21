using Quartz;

namespace RepoRanger.BackgroundJobs.Abstractions.Options;

internal sealed class BackgroundJobOptions
{
    internal const string ConfigurationKey = "BackgroundJobOptions";
    public List<JobOptions> Jobs { get; set; } = [];

    public bool IsEnabled(JobKey jobKey) =>
        Jobs.SingleOrDefault(job => job.JobName == jobKey.Name)?.Enabled ?? false;
    
    public JobKey? NextJobKey(JobKey currentJobKey)
    {
        var jobOptions = Jobs.Single(job => Equals(job.JobKey, currentJobKey));

        return jobOptions.NextJobName is null ? 
            null : 
            new JobKey(jobOptions.NextJobName);
    }
}