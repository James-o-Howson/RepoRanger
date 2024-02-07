using RepoRanger.Application.Sources.Queries.GetSourceDetails;

namespace RepoRanger.App.Services;

public interface IRepoRangerService
{
    Task<SourceDetailsVm?> GetSourceDetailsAsync();
}

internal sealed class RepoRangerService : IRepoRangerService
{
    private readonly HttpClient _httpClient;

    public RepoRangerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SourceDetailsVm?> GetSourceDetailsAsync()
    {
        var response = await _httpClient.GetAsync("/api/sources/");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<SourceDetailsVm>();
    }   
}