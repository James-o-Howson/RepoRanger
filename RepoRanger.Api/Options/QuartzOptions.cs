using RepoRanger.Application.Options;

namespace RepoRanger.Api.Options;

internal sealed class QuartzOptions : IOptions
{
    public string SectionName => "QuartzOptions";
    
    public bool RepoClonerJobEnabled { get; set; }

    public bool IsValid(IHostEnvironment environment) => true;
}