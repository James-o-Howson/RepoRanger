using System.Xml.Linq;
using System.Xml.XPath;
using RepoRanger.Application.ViewModels;

namespace RepoRanger.Infrastructure.AzureDevOps;

public interface IAzureDevOpsRepositoryDataExtractor
{
    Task<SourceViewModel> GetAzureRepositoryDefinitionsAsync();
}

public sealed class AzureDevOpsRepositoryDataExtractor : IAzureDevOpsRepositoryDataExtractor
{
    private const string Source = "AzureDevOps";
    private readonly IAzureDevOpsService _azureDevOpsService;

    public AzureDevOpsRepositoryDataExtractor(IAzureDevOpsService azureDevOpsService)
    {
        _azureDevOpsService = azureDevOpsService;
    }

    public async Task<SourceViewModel> GetAzureRepositoryDefinitionsAsync()
    {
        var projects = await _azureDevOpsService.GetProjectsAsync();
        if (projects is null) throw new ApplicationException("Unable to retrieve Azure Dev Ops Projects");

        var repositoryViewModels = new List<RepositoryViewModel>();
        
        foreach (var project in projects.Value)
        {
            var repositories = await _azureDevOpsService.GetRepositoriesAsync(project.Name);
            if (repositories is null) throw new ApplicationException($"Unable to retrieve Azure Dev Ops Repositories for Project: {project.Name}");

            foreach (var repository in repositories.Value)
            {
                var projectViewModels = new List<ProjectViewModel>();

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
                    
                    var dependencyViewModels = packageReferences.Select(r => new DependencyViewModel(r.Name, r.Version)).ToList();
                    projectViewModels.Add(new ProjectViewModel(project.Name, "", "", dependencyViewModels));
                }
                
                var branchViewModel = new BranchViewModel(repository.DefaultBranch, true, projectViewModels);
                repositoryViewModels.Add(new RepositoryViewModel(repository.Name, repository.Url, repository.RemoteUrl, [branchViewModel]));
            }
        }

        return new SourceViewModel(Source, repositoryViewModels);
    }

    private static IEnumerable<PackageReference> GetPackageReferences(string itemContent)
    {
        var doc = XDocument.Parse(itemContent);
        var packageReferences = doc.XPathSelectElements("//PackageReference")
            .Select(pr => new PackageReference(pr.Attribute("Include").Value.Trim(), pr.Attribute("Version").Value.Trim())).ToList();

        Console.WriteLine($"Project file contains {packageReferences.Count()} package references:");
        foreach (var packageReference in packageReferences)
        {
            Console.WriteLine($"{packageReference.Name}, version {packageReference.Version}");
        }

        return packageReferences;
    }

    private record PackageReference(string Name, string Version);
}