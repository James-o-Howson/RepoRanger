using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Range
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RangeType Type { get; set; }
    
    [JsonPropertyName("repo")]
    public string? Repo { get; set; }
    
    [JsonPropertyName("events")]
    public IEnumerable<Event> Events { get; set; } = null!;
}
