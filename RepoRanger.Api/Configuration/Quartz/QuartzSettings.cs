using RepoRanger.Application.Options;

namespace RepoRanger.Api.Configuration.Quartz;

internal sealed class QuartzSettings : IOptions
{
    public string SectionName => "QuartzSettings";
    
    public bool RepoClonerJobEnabled { get; set; }

    public bool IsValid(IHostEnvironment environment) => true;
}