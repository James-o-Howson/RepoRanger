using RepoRanger.SourceParsing.Services;

namespace RepoRanger.SourceParsing.Configuration;

internal interface ISourceBuilder
{
    ISourceBuilder WithName(string name);
    ISourceBuilder WithWorkingDirectory(string path);
    ISourceBuilder ExcludingRepositories(IEnumerable<string> repositoryNames);
}

internal sealed class SourceBuilder : ISourceBuilder
{
    private readonly SourceOptions _sourceOptions = new();
    
    public ISourceBuilder WithName(string name)
    {
        _sourceOptions.Name = name;
        return this;
    }

    public ISourceBuilder WithWorkingDirectory(string path)
    {
        _sourceOptions.SourceRepositoryParentDirectory = path;
        return this;
    }

    public ISourceBuilder ExcludingRepositories(IEnumerable<string> repositoryNames)
    {
        _sourceOptions.ExcludedRepositories = repositoryNames;
        return this;
    }

    internal SourceOptions Build() => _sourceOptions;
}