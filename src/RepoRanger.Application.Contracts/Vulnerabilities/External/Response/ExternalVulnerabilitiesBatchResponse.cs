namespace RepoRanger.Application.Contracts.Vulnerabilities.External.Response;

public sealed class ExternalVulnerabilitiesBatchResponse
{
    /// <summary>
    /// The Order of the OsvIds list in response is guaranteed to match the order of the query 1-to-1.
    /// </summary>
    public IReadOnlyCollection<List<string>> OsvIds { get; init; } = [];
}