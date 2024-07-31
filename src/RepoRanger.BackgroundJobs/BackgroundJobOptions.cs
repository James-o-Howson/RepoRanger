namespace RepoRanger.BackgroundJobs;

public sealed class BackgroundJobOptions
{
    internal const string ConfigurationKey = "BackgroundJobOptions";
    public bool RepoClonerJobEnabled { get; set; }
}