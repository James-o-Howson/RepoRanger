using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies;
using RepoRanger.Domain.Dependencies.Entities;
using ThirdPartyApiClient;

namespace RepoRanger.Infrastructure.Services;

internal sealed class VulnerabilitiesService : IVulnerabilityService
{
    private readonly IOpenSourceVulnerabilitiesClient _vulnerabilitiesClient;

    public VulnerabilitiesService(IOpenSourceVulnerabilitiesClient vulnerabilitiesClient)
    {
        _vulnerabilitiesClient = vulnerabilitiesClient;
    }

    public async Task<IEnumerable<Vulnerability>> QueryAffectedAsync(string dependencyName, string ecosystem, string? version, CancellationToken cancellationToken = default)
    {
        var result = await _vulnerabilitiesClient.QueryAffectedAsync(new V1Query
        {
            Version = version,
            Package = new OsvPackage
            {
                Name = dependencyName,
                Ecosystem = ecosystem
            },
        }, cancellationToken);
        
        return [];
        // return result.Vulns.Select(v =>
        //     Vulnerability.Create(v.Id, dependencyName, v.Published, v.Withdrawn, v.Summary, v.Details));
    }
}