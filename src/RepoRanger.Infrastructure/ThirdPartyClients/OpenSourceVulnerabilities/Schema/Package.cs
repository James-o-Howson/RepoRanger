using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Package
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("ecosystem")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Ecosystem Ecosystem { get; set; }
    
    [JsonPropertyName("purl")]
    public string? Purl { get; set; }
}
