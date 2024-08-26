using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Contracts.Vulnerabilities.External.Request;
using RepoRanger.Application.Contracts.Vulnerabilities.External.Response;
using ThirdPartyClients.Generated;

namespace RepoRanger.Infrastructure.Vulnerabilities;

internal sealed class ExternalVulnerabilitiesService : IExternalVulnerabilityService
{
    private readonly IOsvClient _vulnerabilitiesClient;

    public ExternalVulnerabilitiesService(IOsvClient vulnerabilitiesClient)
    {
        _vulnerabilitiesClient = vulnerabilitiesClient;
    }

    public async Task<ExternalVulnerabilitiesBatchResponse> BatchQueryAffectedAsync(ExternalVulnerabilityBatchQuery request, CancellationToken cancellationToken = default)
    {
        var query = new V1BatchQuery
        {
            Queries = request.Packages.Select(p => new V1Query
            {
                Version = p.DependencyVersionValue,
                Package = new OsvPackage
                {
                    Name = p.Name,
                    Ecosystem = p.Ecosystem
                }
            }).ToList()
        };
        
        var batchVulnerabilities = await _vulnerabilitiesClient.QueryAffectedBatchAsync(query, cancellationToken);
        
        return new ExternalVulnerabilitiesBatchResponse
        {
            OsvIds = batchVulnerabilities.Results
                .Where(r => r.Vulns != null)
                .SelectMany(l => l.Vulns)
                .Select(v => v.Id)
                .ToList()
        };
    }
}