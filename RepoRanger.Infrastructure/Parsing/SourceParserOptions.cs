using Microsoft.Extensions.Hosting;
using RepoRanger.Application.Abstractions.Options;

namespace RepoRanger.Infrastructure.Parsing;

internal sealed class SourceParserOptions : IOptions
{
    public string SectionName => "SourceParserOptions";

    public List<SourceOptions> Sources { get; set; } = new();
    
    public bool IsValid() =>
        Sources.Count != 0 &&
        Sources.All(s => s.IsValid());
}

public class SourceOptions : IOptions
{
    public string SectionName => "Sources";
    
    public string Name { get; set; } = string.Empty;
    public SourceFetchMethod FetchMethod { get; set; }
    public string SourceRepositoryParentDirectory { get; set; }  = string.Empty;
    public bool Enabled { get; set; }
    
    public bool IsValid() => 
        !string.IsNullOrEmpty(Name);
}

public enum SourceFetchMethod
{
    Api,
    GitSsh
}