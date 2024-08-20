using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;
using ThirdPartyClients.Generated;
using Vulnerability = RepoRanger.Domain.Dependencies.Entities.Vulnerability;

namespace RepoRanger.Infrastructure.Services;

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
            .Select(dependencySource => GetVulnerabilityQuery(dependencyVersion, dependencySource))
            .ToList();

        var batchVulnerabilityList = await _vulnerabilitiesClient.QueryAffectedBatchAsync(new V1BatchQuery
        {
            Queries = queries,
        }, cancellationToken);

        return GetVulnerabilities(batchVulnerabilityList, dependencyVersion.Id);
    }

    private static IEnumerable<Vulnerability> GetVulnerabilities(V1BatchVulnerabilityList batchVulnerabilityList, DependencyVersionId id) =>
        batchVulnerabilityList.Results.SelectMany(r => r.Vulns)
            .Select(v => Vulnerability.Create(v.Id, id, v.Published, v.Withdrawn, v.Summary, v.Details));

    private static V1Query GetVulnerabilityQuery(DependencyVersion dependencyVersion, DependencySource dependencySource) =>
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