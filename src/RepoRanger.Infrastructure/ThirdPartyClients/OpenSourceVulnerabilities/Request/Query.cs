using System.Text.Json.Serialization;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Request;

public sealed class Query
{
    [JsonPropertyName("commit")]
    public string? Commit { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("package")]
    public Package Package { get; set; } = null!;
}
