using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Request;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Response;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;
using RestSharp;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities;

internal interface IOsvClient
{
    Task<VulnerabilityList> QueryAffectedAsync(Query query, CancellationToken cancellationToken = default);

    Task<BatchVulnerabilityList> QueryAffectedBatchAsync(BatchQuery batchQuery, CancellationToken cancellationToken = default);

    Task<Vulnerability> GetVulnerabilityAsync(string id, CancellationToken cancellationToken = default);
}

internal sealed class OsvClient : BaseApi, IOsvClient
{
    public OsvClient(HttpClient httpClient) : base(httpClient) { }
    
    public Task<VulnerabilityList> QueryAffectedAsync(Query query, CancellationToken cancellationToken = default)
    {
        const string resource = "query";
        
        var request = new RestRequest(resource, Method.Post)
        {
            RequestFormat = DataFormat.Json,
        }.AddBody(query);
        
        return ExecuteAsync<VulnerabilityList>(request, cancellationToken);
    }
    
    public Task<BatchVulnerabilityList> QueryAffectedBatchAsync(BatchQuery batchQuery, CancellationToken cancellationToken = default)
    {
        const string resource = "querybatch";
        
        var request = new RestRequest(resource, Method.Post)
        {
            RequestFormat = DataFormat.Json,
        }.AddBody(batchQuery);
        
        return ExecuteAsync<BatchVulnerabilityList>(request, cancellationToken);
    }
    
    public Task<Vulnerability> GetVulnerabilityAsync(string id, CancellationToken cancellationToken = default)
    {
        const string resource = "vulns/{id}";
        const string urlSegmentName = "id";
        var request = new RestRequest(resource).AddUrlSegment(urlSegmentName, id);
        
        return ExecuteAsync<Vulnerability>(request, cancellationToken);
    }
}
