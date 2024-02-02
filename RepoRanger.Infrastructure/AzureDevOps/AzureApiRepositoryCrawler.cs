using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Infrastructure.AzureDevOps.Models.Items;
using RepoRanger.Infrastructure.AzureDevOps.Models.Projects;
using RepoRanger.Infrastructure.AzureDevOps.Models.Repositories;
using RepoRanger.Infrastructure.FileParsing.CSharp;

namespace RepoRanger.Infrastructure.AzureDevOps;

public interface IAzureDevOpsRepositoryDataExtractor
{
    Task<SourceDto> GetAzureRepositoriesAsync();
}

public sealed class AzureDevOpsRepositoryDataExtractor : IAzureDevOpsRepositoryDataExtractor
{
    private const string Source = "AzureDevOps";
    
    private readonly IAzureDevOpsService _azureDevOpsService;
    private readonly ILogger<AzureDevOpsRepositoryDataExtractor> _logger;
    private readonly ICsprojDependencyFileParser _dependencyFileParser;
    private readonly ICsprojDotNetVersionFileParser _dotNetVersionFileParser;

    public AzureDevOpsRepositoryDataExtractor(IAzureDevOpsService azureDevOpsService,
        ILogger<AzureDevOpsRepositoryDataExtractor> logger,
        ICsprojDependencyFileParser dependencyFileParser,
        ICsprojDotNetVersionFileParser dotNetVersionFileParser)
    {
        _azureDevOpsService = azureDevOpsService;
        _logger = logger;
        _dependencyFileParser = dependencyFileParser;
        _dotNetVersionFileParser = dotNetVersionFileParser;
    }

    public async Task<SourceDto> GetAzureRepositoriesAsync()
    {
        var projects = await _azureDevOpsService.GetProjectsAsync();
        if (projects is null) throw new ApplicationException("Unable to retrieve Azure Dev Ops Projects");

        var tasks = projects.Value.Select(async p => await CreateRepositories(p));
        var repositoryViewModels = (await Task.WhenAll(tasks)).SelectMany(v => v);
        
        return new SourceDto(Source, repositoryViewModels);
    }

    private async Task<List<RepositoryDto>> CreateRepositories(AzureDevOpsProject project)
    {
        var repositories = await _azureDevOpsService.GetRepositoriesAsync(project.Name);
        if (repositories is null) throw new ApplicationException($"Unable to retrieve Azure Dev Ops Repositories for Project: {project.Name}");

        var repositoryViewModels = new List<RepositoryDto>();
        foreach (var repository in repositories.Value)
        {
            var items = await _azureDevOpsService.GetItemsAsync(project.Name, repository.Id);

            var solutionItemDefinitions = items?.Value.Where(i => !i.IsFolder && i.Path.EndsWith(".sln")).ToList();
            var projectItemDefinitions = items?.Value.Where(i => !i.IsFolder && i.Path.EndsWith(".csproj")).ToList();

            if (projectItemDefinitions is null || projectItemDefinitions.Count <= 0) continue;
                
            var repositoryViewModel = await CreateRepository(projectItemDefinitions, project, repository);
            repositoryViewModels.Add(repositoryViewModel);
        }

        return repositoryViewModels;
    }

    private async Task<RepositoryDto> CreateRepository(List<AzureDevOpsItem> projectItemDefinitions, AzureDevOpsProject project,
        AzureDevOpsRepository repository)
    {
        var projectViewModels = await CreateProjects(projectItemDefinitions, project, repository);
                
        var branchViewModel = new BranchDto(repository.DefaultBranch, true, projectViewModels);
        return new RepositoryDto(repository.Name, repository.Url, repository.RemoteUrl, [branchViewModel]);
    }

    private async Task<List<ProjectDto>> CreateProjects(List<AzureDevOpsItem> projectItemDefinitions, AzureDevOpsProject project,
        AzureDevOpsRepository repository)
    {
        var projectViewModels = new List<ProjectDto>();

        foreach (var projectItemDefinition in projectItemDefinitions)
        {
            var itemContent =
                await _azureDevOpsService.GetItemAsync(project.Name, repository.Id, projectItemDefinition.Path);

            if (string.IsNullOrEmpty(itemContent)) continue;
            projectViewModels.Add(CreateProject(itemContent, project));
        }

        return projectViewModels;
    }

    private ProjectDto CreateProject(string itemContent, AzureDevOpsProject project)
    {
        var dependencyViewModels = _dependencyFileParser.Parse(itemContent);
        var dotnetVersion = _dotNetVersionFileParser.Parse(itemContent);
                    
        return new ProjectDto(project.Name, dotnetVersion, dependencyViewModels);
    }

    private ProjectMetadata GetProjectMetadata(string itemContent)
    {
        var doc = XDocument.Parse(itemContent);
        var element = doc.XPathSelectElement("//PackageReference");
        var type = element.Attribute("Include")?.Value.Trim() ?? string.Empty;
        var version = element.Attribute("Version")?.Value.Trim() ?? string.Empty;
        var projectMetadata = new ProjectMetadata(type, version);

        return projectMetadata;
    }

    private record ProjectMetadata(string Type, string Version);
}