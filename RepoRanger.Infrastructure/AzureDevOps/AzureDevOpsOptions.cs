using System.Net.Http.Headers;
using System.Text;

namespace RepoRanger.Infrastructure.AzureDevOps;

public sealed class AzureDevOpsOptions
{
    public string BaseAddress { get; set; } = string.Empty;
    public string PersonalAccessToken { get; set; } = string.Empty;
    public string Organisation { get; set; } = string.Empty;

    public AuthenticationHeaderValue AuthenticationHeader() =>
        new("Basic", 
            Convert.ToBase64String(Encoding.ASCII.GetBytes($":{PersonalAccessToken}")));
    
    public Uri BaseAddressUri() => new(BaseAddress);
}