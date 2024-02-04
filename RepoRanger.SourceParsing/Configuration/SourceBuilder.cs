using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Abstractions.Exceptions;
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
    private readonly IServiceCollection _services;
    private readonly SourceOptions _sourceOptions = new();

    public SourceBuilder(IServiceCollection services)
    {
        _services = services;
    }

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

    internal SourceOptions Build()
    {
        _services.Configure<SourceParserOptions>(parserOptions =>
        {
            if (!parserOptions.SourcesEnabledViaConfiguration) return;
            
            if (string.IsNullOrEmpty(_sourceOptions.Name))
                throw new InvalidOperationException("Cannot enable source via configuration with no source name set");
            
            var succeeded = parserOptions.SourceEnabledByName.TryGetValue(_sourceOptions.Name, out var enabled);
            if (!succeeded) throw new NotFoundException($"Unable to find enabled configuration for source: {_sourceOptions.Name}");
            _sourceOptions.Enabled = enabled;
        });

        return _sourceOptions;
    }
}