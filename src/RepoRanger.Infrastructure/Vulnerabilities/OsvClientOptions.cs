namespace RepoRanger.Infrastructure.Vulnerabilities;

internal sealed class OsvClientOptions
{
    internal const string SectionKey = "OsvClientOptions";

    public required string BaseUrl { get; init; }
    
    public Uri BaseUrlUri => new(BaseUrl);
}