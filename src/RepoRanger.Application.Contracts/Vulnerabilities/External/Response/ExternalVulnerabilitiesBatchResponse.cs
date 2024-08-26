namespace RepoRanger.Application.Contracts.Vulnerabilities.External.Response;

public sealed class ExternalVulnerabilitiesBatchResponse
{
    public IReadOnlyCollection<string> OsvIds { get; set; } = [];
}