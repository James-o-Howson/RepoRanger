using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Severity
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<SeverityType>))]
    public SeverityType Type { get; set; }
    
    [JsonPropertyName("score")]
    public string Score { get; set; } = null!;
}
