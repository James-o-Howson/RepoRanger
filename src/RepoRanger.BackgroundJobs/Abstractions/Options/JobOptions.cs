using Quartz;

namespace RepoRanger.BackgroundJobs.Abstractions.Options;

internal sealed class JobOptions
{
    public required bool Enabled { get; init; }
    public required string JobName { get; init; }
    public string? NextJobName { get; init; }
    
    internal JobKey JobKey => new(JobName);
}