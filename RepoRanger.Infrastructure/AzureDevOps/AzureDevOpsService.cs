using System.Text.Json;
using Microsoft.Extensions.Options;
using RepoRanger.Infrastructure.AzureDevOps.Models.Items;
using RepoRanger.Infrastructure.AzureDevOps.Models.Projects;
using RepoRanger.Infrastructure.AzureDevOps.Models.Repositories;

namespace RepoRanger.Infrastructure.AzureDevOps;

public interface IAzureDevOpsService
{
    Task<AzureDevOpsProjects?> GetProjectsAsync();
    Task<AzureDevOpsRepositories?> GetRepositoriesAsync(string projectName);
    Task<AzureDevOpsItems?> GetItemsAsync(string projectName, string repositoryId);
    Task<string?> GetItemAsync(string projectName, string repositoryId, string path);
}

public sealed class AzureDevOpsService : IAzureDevOpsService
{
    private readonly AzureDevOpsOptions _azureDevOpsOptions;
    private readonly HttpClient _httpClient;

    public AzureDevOpsService(HttpClient httpClient, IOptions<AzureDevOpsOptions> options)
    {
        _httpClient = httpClient;
        _azureDevOpsOptions = options.Value;
    }

    public async Task<AzureDevOpsProjects?> GetProjectsAsync()
    {
        var requestUri = $"/{_azureDevOpsOptions.Organisation}/_apis/projects";
        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        var projects = JsonSerializer.Deserialize<AzureDevOpsProjects>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return projects;
    }

    public async Task<AzureDevOpsRepositories?> GetRepositoriesAsync(string projectName)
    {
        var requestUri = $"/{_azureDevOpsOptions.Organisation}/{projectName}/_apis/git/repositories";
        var response = await _httpClient.GetAsync(requestUri);
        
        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadAsStringAsync();
        var repositories = JsonSerializer.Deserialize<AzureDevOpsRepositories>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return repositories;
    }

    public async Task<AzureDevOpsItems?> GetItemsAsync(string projectName, string repositoryId)
    {
        var requestUri = $"/{_azureDevOpsOptions.Organisation}/{projectName}/_apis/git/repositories/{repositoryId}/items?recursionLevel=Full";
        var response = await _httpClient.GetAsync(requestUri);
        
        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadAsStringAsync();
        var items = JsonSerializer.Deserialize<AzureDevOpsItems>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return items;
    }

    public async Task<string?> GetItemAsync(string projectName, string repositoryId, string path)
    {
        var requestUri = $"/{_azureDevOpsOptions.Organisation}/{projectName}/_apis/git/repositories/{repositoryId}/items?path={path}";
        var response = await _httpClient.GetAsync(requestUri);
        
        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
}