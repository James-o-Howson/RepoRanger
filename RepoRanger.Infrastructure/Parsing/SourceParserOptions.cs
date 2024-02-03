using Microsoft.Extensions.Hosting;
using RepoRanger.Application.Abstractions.Options;

namespace RepoRanger.Infrastructure.Parsing;

internal sealed class SourceParserOptions : IOptions
{
    public string SectionName => "SourceParserOptions";

    public List<SourceOptions> Sources { get; set; }
    
    public bool IsValid(IHostEnvironment environment) =>
        Sources.Count != 0 &&
        Sources.All(s => s.IsValid(environment));
}

public class SourceOptions : IOptions
{
    public string SectionName => "Sources";
    
    public string Name { get; set; }
    public SourceFetchMethod FetchMethod { get; set; }
    public string SourceRepositoryParentDirectory { get; set; }
    public bool Enabled { get; set; }
    
    public bool IsValid(IHostEnvironment environment) => 
        !string.IsNullOrEmpty(Name);
}

public enum SourceFetchMethod
{
    Api,
    GitSsh
}