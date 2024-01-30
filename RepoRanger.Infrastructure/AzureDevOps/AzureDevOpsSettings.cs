using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Hosting;
using RepoRanger.Application.Options;

namespace RepoRanger.Infrastructure.AzureDevOps;

public sealed class AzureDevOpsSettings : IOptions
{
    public string SectionName => "AzureDevOpsSettings";
    
    public string BaseAddress { get; set; }
    public string PersonalAccessToken { get; set; }
    public string Organisation { get; set; }

    public AuthenticationHeaderValue AuthenticationHeader() =>
        new("Basic", 
            Convert.ToBase64String(Encoding.ASCII.GetBytes($":{PersonalAccessToken}")));
    
    public Uri BaseAddressUri() => new(BaseAddress);

    public bool IsValid(IHostEnvironment environment)
    {
        return !string.IsNullOrEmpty(BaseAddress) &&
               !string.IsNullOrEmpty(Organisation) &&
               !string.IsNullOrEmpty(PersonalAccessToken);
    }
}