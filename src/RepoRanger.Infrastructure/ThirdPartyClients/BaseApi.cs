using RestSharp;

namespace RepoRanger.Infrastructure.ThirdPartyClients;

internal class BaseApi : IDisposable
{
    private readonly RestClient _client;

    protected BaseApi(HttpClient httpClient)
    {
        _client = new RestClient(httpClient);
    }

    public void Dispose() => _client.Dispose();
    
    protected async Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : new()
    {
        T data;

        try
        {
            var response = await _client.ExecuteAsync<T>(request, cancellationToken);
            if (!response.IsSuccessful)
            {
                throw new ThirdPartyApiException($"{response.StatusCode}: {response.ErrorMessage}");
            }

            data = response.ThrowIfError().Data!;
        }
        catch (Exception ex)
        {
            throw new ThirdPartyApiException(ex.Message, ex);
        }

        return data;
    }
}