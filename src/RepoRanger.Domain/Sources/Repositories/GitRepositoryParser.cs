using RepoRanger.Domain.Common.Interfaces;
using RepoRanger.Domain.Sources.Repositories.Git;

namespace RepoRanger.Domain.Sources.Repositories;

public interface IGitRepositoryParser
{
    Task<Repository> ParseAsync(ParsingContext parsingContext);
}

internal sealed class GitRepositoryParser : IGitRepositoryParser
{
    private readonly IGitDetailService _gitDetailService;

    public GitRepositoryParser(IGitDetailService gitDetailService)
    {
        _gitDetailService = gitDetailService;
    }

    public async Task<Repository> ParseAsync(ParsingContext parseContext)
    {
        parseContext.EnsureParsingContextValid();
        
        var repository = Create(parseContext.GitDirectory!, parseContext.SourceId!.Value);
        
        var filePaths = Directory.EnumerateFiles(parseContext.GitDirectoryPath, "*.*", SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
        
        var parseFileTasks = filePaths.Select(filePath => 
            AddProjectsAsync(repository, filePath, parseContext.SourceFileParsers));
        
        await Task.WhenAll(parseFileTasks);
        
        return repository;
    }
    
    private Repository Create(DirectoryInfo gitDirectory, Guid sourceId)
    {
        var detail = _gitDetailService.GetRepositoryDetail(gitDirectory);
        var repository = Repository.Create(detail.Name, detail.RemoteUrl, detail.BranchName, sourceId);
        
        return repository;
    }
    
    private static async Task AddProjectsAsync(Repository repository, string filePath,
        IEnumerable<ISourceFileParser> sourceFileParsers)
    {
        var fileContentParser = sourceFileParsers.SingleOrDefault(p => p.CanParse(filePath));
        if (fileContentParser != null)
        {
            // Don't read content or make file info unless the file path matches a parser, it's expensive.
            var content = await File.ReadAllTextAsync(filePath);
            var fileInfo = new FileInfo(filePath);

            var projects = await fileContentParser.ParseAsync(content, fileInfo);
            repository.AddProjects(projects.ToList());
        }
    }
}