using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public sealed class Credit
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("contact")]
    public IEnumerable<string> Contact { get; set; } = null!;
}
