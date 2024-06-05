using RepoRanger.Domain.Entities;
using RepoRanger.Domain.Git;
using RepoRanger.Domain.SourceParsing.Common;

namespace RepoRanger.Domain.SourceParsing;

public interface IRepositoryParser
{
    Task<Repository> ParseAsync(DirectoryInfo gitRepository, ParsingContext parsingContext);
}

internal sealed class RepositoryParser : IRepositoryParser
{
    private readonly IGitDetailService _gitDetailService;

    public RepositoryParser(IGitDetailService gitDetailService)
    {
        _gitDetailService = gitDetailService;
    }

    public async Task<Repository> ParseAsync(DirectoryInfo gitRepository, ParsingContext parseContext)
    {
        ArgumentNullException.ThrowIfNull(gitRepository);
        
        var filePaths = Directory.EnumerateFiles(gitRepository.FullName, "*.*", SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
        
        var repository = CreateRepository(gitRepository);

        var parseFileTasks = filePaths.Select(filePath => 
            AddProjectsAsync(gitRepository, filePath, parseContext));
        
        var projects = (await Task.WhenAll(parseFileTasks)).SelectMany(p => p);
        repository.AddProjects(projects.ToList());
        
        return repository;
    }
    
    private Repository CreateRepository(DirectoryInfo gitDirectory)
    {
        var detail = _gitDetailService.GetRepositoryDetail(gitDirectory);
        
        return Repository.Create(detail.Name, detail.RemoteUrl, detail.BranchName);
    }
    
    private static async Task<List<Project>> AddProjectsAsync(DirectoryInfo gitRepository, string filePath,
        ParsingContext parsingContext)
    {
        List<Project> projects = [];
        var fileContentParser = parsingContext.SourceFileParsers
            .SingleOrDefault(p => p.CanParse(filePath));
        
        if (fileContentParser == null) return projects;
        
        // Don't read content or make file info unless the file path matches a parser, it's expensive.
        var fileInfo = new FileInfo(filePath);  

        if (parsingContext.IsAlreadyParsed(filePath)) return projects;
        
        projects = (await fileContentParser.ParseAsync(gitRepository, fileInfo, parsingContext)).ToList();
        parsingContext.MarkAsParsed(filePath, fileInfo);

        return projects;
    }
}