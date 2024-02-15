using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;
using RepoRanger.Application.Sources.Queries.ListSources;

namespace RepoRanger.Api.Services;

internal sealed class RepoRangerService : IRepoRangerService
{
    private readonly HttpClient _httpClient;

    public RepoRangerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SourcesVm?> ListSources()
    {
        var response = await _httpClient.GetAsync("/api/sources/");
    
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<SourcesVm>();
    }   
    
    public async Task<RepositoriesVm?> GetRepositoriesAsync(GetRepositoriesBySourceIdQuery query)
    {
        var content = JsonContent.Create(query);
        var response = await _httpClient.PostAsync("/api/repositories/", content);
    
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<RepositoriesVm>();
    }
}