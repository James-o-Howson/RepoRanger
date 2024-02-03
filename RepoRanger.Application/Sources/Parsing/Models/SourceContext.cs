namespace RepoRanger.Application.Sources.Parsing.Models;

public interface ISourceContext
{
}

public sealed class SourceContext : ISourceContext
{
    public string SourceName { get; init; }
    public IEnumerable<RepositoryContext> RepositoryContexts { get; set;  } = [];
    
}