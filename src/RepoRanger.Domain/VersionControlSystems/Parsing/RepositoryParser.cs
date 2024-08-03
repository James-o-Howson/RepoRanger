using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.VersionControlSystems.Git;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public interface IRepositoryParser
{
    Task<RepositoryDescriptor> ParseAsync(DirectoryInfo gitRepository, ParsingContext parsingContext);
}

internal sealed class RepositoryParser : IRepositoryParser
{
    private readonly IGitRepositoryDetailFactory _gitRepositoryDetailFactory;

    public RepositoryParser(IGitRepositoryDetailFactory gitRepositoryDetailFactory)
    {
        _gitRepositoryDetailFactory = gitRepositoryDetailFactory;
    }

    public async Task<RepositoryDescriptor> ParseAsync(DirectoryInfo gitRepository, ParsingContext parseContext)
    {
        ArgumentNullException.ThrowIfNull(gitRepository);
        
        var filePaths = Directory.EnumerateFiles(gitRepository.FullName, "*.*", SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
        
        var parseFileTasks = filePaths.Select(filePath => 
            AddProjectsAsync(gitRepository, filePath, parseContext));
        
        var projects = (await Task.WhenAll(parseFileTasks)).SelectMany(p => p);
        
        var repository = CreateRepository(gitRepository, projects.ToList());
        return repository;
    }
    
    private RepositoryDescriptor CreateRepository(DirectoryInfo gitDirectory, IReadOnlyCollection<ProjectDescriptor> projectDescriptors)
    {
        var detail = _gitRepositoryDetailFactory.Create(gitDirectory);
        
        return new RepositoryDescriptor(detail.Name, detail.RemoteUrl, detail.BranchName, projectDescriptors);
    }
    
    private static async Task<IReadOnlyCollection<ProjectDescriptor>> AddProjectsAsync(DirectoryInfo gitRepository, string filePath,
        ParsingContext parsingContext)
    {
        IReadOnlyCollection<ProjectDescriptor> projectDescriptors = [];
        var fileContentParser = parsingContext.FileParsers
            .SingleOrDefault(p => p.CanParse(filePath));
        
        if (fileContentParser == null) return [];
        
        // Don't read content or make file info unless the file path matches a parser, it's expensive.
        var fileInfo = new FileInfo(filePath);  

        if (parsingContext.IsAlreadyParsed(filePath)) return projectDescriptors;

        parsingContext.MarkAsParsed(filePath, fileInfo);
        projectDescriptors = await fileContentParser.ParseAsync(gitRepository, fileInfo, parsingContext);

        return parsingContext.IsAlreadyParsed(filePath) ? [] : projectDescriptors;
    }
}