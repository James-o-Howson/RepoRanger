using Quartz;

namespace RepoRanger.BackgroundJobs.Abstractions.Options;

internal sealed class JobOptions
{
    public bool Enabled { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string? NextJobName { get; set; }
    
    internal JobKey JobKey => new(JobName);
}