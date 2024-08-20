using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Affected
{
    [JsonPropertyName("package")]
    public Package Package { get; set; } = null!;

    [JsonPropertyName("ranges")]
    public IEnumerable<Range> Ranges { get; set; } = null!;

    [JsonPropertyName("versions")]
    public IEnumerable<string>? Versions { get; set; }

    [JsonPropertyName("ecosystemSpecific")]
    public Dictionary<string, object>? EcosystemSpecific { get; set; }

    [JsonPropertyName("databaseSpecific")]
    public Dictionary<string, object>? DatabaseSpecific { get; set; }
}
