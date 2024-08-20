using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Event
{
    [JsonPropertyName("introduced")]
    public string? Introduced { get; set; }
    
    [JsonPropertyName("fixed")]
    public string? Fixed { get; set; }
    
    [JsonPropertyName("limit")]
    public string? Limit { get; set; }
}
