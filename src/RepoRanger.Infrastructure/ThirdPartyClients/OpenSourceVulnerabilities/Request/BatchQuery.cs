using System.Text.Json.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Request;

public sealed class BatchQuery
{
    [JsonPropertyName("queries")]
    public IEnumerable<Query> Queries { get; set; } = null!;
}
