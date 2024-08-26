namespace RepoRanger.Application.Contracts.Vulnerabilities.External.Response;

public sealed class ExternalVulnerabilitiesResponse
{
    public IReadOnlyCollection<ExternalVulnerability> Vulnerabilities { get; set; } = [];
}