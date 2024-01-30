using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Options;
using Quartz;
using RepoRanger.Api.Configuration.Quartz;
using RepoRanger.Infrastructure.AzureDevOps;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class RepositoryDataCollectorJob : IJob
{
    private readonly ILogger<RepositoryDataCollectorJob> _logger;
    private readonly QuartzSettings _quartzSettings;
    private readonly IAzureDevOpsService _azureDevOpsService;

    public RepositoryDataCollectorJob(ILogger<RepositoryDataCollectorJob> logger, IOptions<QuartzSettings> options, IAzureDevOpsService azureDevOpsService)
    {
        _logger = logger;
        _azureDevOpsService = azureDevOpsService;
        _quartzSettings = options.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Repo Cloner Job - Starting");

            if (!_quartzSettings.RepoClonerJobEnabled)
            {
                _logger.LogInformation("Repo Cloner Job Disabled - Skipping");
                return;
            }

            await GetAzureRepositoryDefinitions();

            _logger.LogInformation("Repo Cloner Job - Finished");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Repo Cloner Job - Error");
            context.Result = e;
        }
    }

    private async Task GetAzureRepositoryDefinitions()
    {
        var projects = await _azureDevOpsService.GetProjectsAsync();
        if (projects is null) throw new ApplicationException("Unable to retrieve Azure Dev Ops Projects");
            
        foreach (var project in projects.Value)
        {
            var repositories = await _azureDevOpsService.GetRepositoriesAsync(project.Name);
            if (repositories is null) throw new ApplicationException($"Unable to retrieve Azure Dev Ops Repositories for Project: {project.Name}");

            foreach (var repository in repositories.Value)
            {
                var items = await _azureDevOpsService.GetItemsAsync(project.Name, repository.Id);

                var solutionItemDefinitions = items?.Value.Where(i => !i.IsFolder && i.Path.EndsWith(".sln")).ToList();
                var projectItemDefinitions = items?.Value.Where(i => !i.IsFolder && i.Path.EndsWith(".csproj")).ToList();

                if (projectItemDefinitions is null || projectItemDefinitions.Count <= 0) continue;
                foreach (var projectItemDefinition in projectItemDefinitions)
                {
                    var itemContent =
                        await _azureDevOpsService.GetItemAsync(project.Name, repository.Id, projectItemDefinition.Path);

                    if (string.IsNullOrEmpty(itemContent)) continue;

                    var packageReferences = GetPackageReferences(itemContent);
                }
            }
        }
    }

    private List<PackageReference> GetPackageReferences(string itemContent)
    {
        var doc = XDocument.Parse(itemContent);
        var packageReferences = doc.XPathSelectElements("//PackageReference")
            .Select(pr => new PackageReference(pr.Attribute("Include").Value, new Version(pr.Attribute("Version").Value))).ToList();

        Console.WriteLine($"Project file contains {packageReferences.Count()} package references:");
        foreach (var packageReference in packageReferences)
        {
            Console.WriteLine($"{packageReference.Include}, version {packageReference.Version}");
        }

        return packageReferences;
    }

    private record PackageReference(string Include, Version Version);
}