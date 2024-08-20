using System.Text.Json.Serialization;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

namespace OSV.Schema;

public sealed class Reference
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReferenceType Type { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;
}
