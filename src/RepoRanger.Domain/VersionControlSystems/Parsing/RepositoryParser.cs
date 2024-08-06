using RepoRanger.Domain.VersionControlSystems.Git;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contexts;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Parsing;

internal interface IRepositoryParser
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

        List<ProjectDescriptor> projectDescriptors = [];
        foreach (var filePath in filePaths)
        {
            projectDescriptors.AddRange(await TryExtractProjectsAsync(gitRepository, filePath, parseContext));
        }
        
        return CreateRepository(gitRepository, projectDescriptors);
    }
    
    private RepositoryDescriptor CreateRepository(DirectoryInfo gitDirectory, IReadOnlyCollection<ProjectDescriptor> projectDescriptors)
    {
        var detail = _gitRepositoryDetailFactory.Create(gitDirectory);
        
        return new RepositoryDescriptor(detail.Name, detail.RemoteUrl, detail.BranchName, projectDescriptors);
    }
    
    private static async Task<IReadOnlyCollection<ProjectDescriptor>> TryExtractProjectsAsync(DirectoryInfo gitRepository, string filePath,
        ParsingContext parsingContext)
    {
        IReadOnlyCollection<ProjectDescriptor> projectDescriptors = [];
        var fileContentParser = parsingContext.FileParsers
            .SingleOrDefault(p => p.CanParse(filePath));
        
        if (fileContentParser == null) return [];
        
        // Don't read content or make file info unless the file path matches a parser, it's expensive.
        var fileInfo = new FileInfo(filePath);  

        if (parsingContext.IsAlreadyParsed(filePath)) return projectDescriptors;

        projectDescriptors = await fileContentParser.ParseAsync(gitRepository, fileInfo, parsingContext);
        parsingContext.MarkAsParsed(filePath, fileInfo);

        return projectDescriptors;
    }
}