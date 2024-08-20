using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Request;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Response;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;
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
            .Select(dependencySource => GetVulnerabilityQuery(dependencyVersion, dependencySource));

        var batchVulnerabilityList = await _vulnerabilitiesClient.QueryAffectedBatchAsync(new BatchQuery
        {
            Queries = queries,
        }, cancellationToken);

        return GetVulnerabilities(batchVulnerabilityList, dependencyVersion.Id);
    }

    private static IEnumerable<Vulnerability> GetVulnerabilities(BatchVulnerabilityList batchVulnerabilityList, DependencyVersionId id) =>
        batchVulnerabilityList.Results.SelectMany(r => r.Vulnerabilities)
            .Select(v => Vulnerability.Create(v.Id, id, v.Published, v.Withdrawn, v.Summary, v.Details));

    private static Query GetVulnerabilityQuery(DependencyVersion dependencyVersion, DependencySource dependencySource) =>
        new()
        {
            Version = dependencyVersion.Value,
            Package = new Package
            {
                Name = dependencyVersion.Dependency.Name,
                Ecosystem = GetVulnerabilityEcosystem(dependencySource)
            }
        };

    private static Ecosystem GetVulnerabilityEcosystem(DependencySource dependencySource)
    {
        var ecosystemValue = dependencySource.Name;
        var successful = Enum.TryParse(ecosystemValue, true, out Ecosystem ecosystem);
        
        if(successful) return ecosystem;
        
        throw new ApplicationException($"Invalid ecosystem value: {ecosystemValue}");
    }
}