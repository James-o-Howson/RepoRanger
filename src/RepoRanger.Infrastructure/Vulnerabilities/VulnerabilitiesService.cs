using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies.Entities;
using ThirdPartyClients.Generated;
using Vulnerability = RepoRanger.Domain.Dependencies.Entities.Vulnerability;

namespace RepoRanger.Infrastructure.Vulnerabilities;

internal sealed class VulnerabilitiesService : IVulnerabilityService
{
    private readonly IOsvClient _vulnerabilitiesClient;

    public VulnerabilitiesService(IOsvClient vulnerabilitiesClient)
    {
        _vulnerabilitiesClient = vulnerabilitiesClient;
    }

    public async Task<IEnumerable<Vulnerability>> QueryVulnerabilitiesAsync(DependencyVersion dependencyVersion, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dependencyVersion);
        ArgumentNullException.ThrowIfNull(dependencyVersion.Dependency);
        ArgumentNullException.ThrowIfNull(dependencyVersion.Sources);

        var queries = dependencyVersion.Sources
            .Select(dependencySource => CreateVulnerabilityQuery(dependencyVersion, dependencySource))
            .ToList();

        var batchVulnerabilityList = await _vulnerabilitiesClient.QueryAffectedBatchAsync(new V1BatchQuery
        {
            Queries = queries,
        }, cancellationToken);

        return batchVulnerabilityList.ToVulnerabilities(dependencyVersion.Id);
    }
    
    private static V1Query CreateVulnerabilityQuery(DependencyVersion dependencyVersion, DependencySource dependencySource) =>
        new()
        {
            Version = dependencyVersion.Value,
            Package = new OsvPackage
            {
                Name = dependencyVersion.Dependency.Name,
                Ecosystem = dependencySource.Name
            }
        };
}